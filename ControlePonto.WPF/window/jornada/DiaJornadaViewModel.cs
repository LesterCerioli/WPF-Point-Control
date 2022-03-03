using ControlePonto.Domain.jornada;
using ControlePonto.Infrastructure.utils;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.WPF.window.jornada
{
    public class DiaJornadaViewModel : ViewModelBase
    {
        protected internal DiaJornada dia;
        private JornadaTrabalho jornada;

        private static TimeSpan ENTRADA_PADRAO { get { return new TimeSpan(9, 0, 0); } }
        private static TimeSpan SAIDA_PADRAO { get { return new TimeSpan(18, 0, 0); } }
        private static TimeSpan FOLGA_PADRAO { get { return new TimeSpan(1, 0, 0); } }

        protected internal DiaJornadaViewModel(JornadaTrabalho jornada, DayOfWeek day)
        {
            this.jornada = jornada;
            this.dia = jornada.getDia(day);

            this.Entrada = dia.EntradaEsperada;
            this.Saida = dia.SaidaEsperada;
            this.Folga = dia.FolgaEsperada;
            this._diaDeTrabalho = !nenhumHorarioFoiDefinido();            
        }

        private bool nenhumHorarioFoiDefinido()
        {
            return dia.EntradaEsperada == JornadaTrabalho.NAO_DEFINIDO &&
                dia.SaidaEsperada == JornadaTrabalho.NAO_DEFINIDO &&
                dia.FolgaEsperada == JornadaTrabalho.NAO_DEFINIDO;
        }

        #region Propriedades

        public string NomeDiaSemana
        {
            get
            {
                return DiaSemanaTradutor.traduzir(dia.DiaSemana);
            }
        }

        private bool _diaDeTrabalho;
        public bool DiaDeTrabalho
        {
            get { return _diaDeTrabalho; }
            set 
            { 
                if (SetField(ref _diaDeTrabalho, value))
                {
                    if (value)
                    {
                        Entrada = ENTRADA_PADRAO;
                        Saida = SAIDA_PADRAO;
                        Folga = FOLGA_PADRAO;
                    }
                    else
                    {
                        Entrada = JornadaTrabalho.NAO_DEFINIDO;
                        Saida = JornadaTrabalho.NAO_DEFINIDO;
                        Folga = JornadaTrabalho.NAO_DEFINIDO;
                    }
                }
            }
        }
        
        private TimeSpan _entrada;
        public TimeSpan Entrada
        {
            get { return _entrada; }
            set 
            { 
                if(SetField(ref _entrada, value))
                {
                    RaisePropertyChanged("Saida"); //Atualizar mensagem de erro
                }
            }
        }

        private TimeSpan _saida;
        public TimeSpan Saida
        {
            get { return _saida; }
            set 
            {
                if (SetField(ref _saida, value))
                {
                    RaisePropertyChanged("Entrada"); //Atualizar mensagem de erro
                }
            }
        }
        
        private TimeSpan _folga;
        public TimeSpan Folga
        {
            get { return _folga; }
            set { SetField(ref _folga, value); }
        }

        #endregion

        protected override string validar(string propertyName)
        {
            switch(propertyName)
            {
                case "Entrada":
                    if (Saida < Entrada)
                        return "O horário de entrada deve ser antes do de saída";
                    break;

                case "Saida":
                    if (Saida < Entrada)
                        return "O horário de saída deve ser após o de entrada";
                    break;
            }
            return null;
        }
    }
}
