using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.jornada;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto.trabalho
{
    public abstract class DiaTrabalho : PontoDia
    {
        #region Propriedades

        private TimeSpan _Inicio { get; set; }
        public virtual TimeSpan Inicio
        {
            get { return _Inicio; }
            set
            {
                _Inicio = value;
            }
        }


        private TimeSpan? _Fim;
        public virtual TimeSpan? Fim
        {
            get { return _Fim; }
            set
            {
                _Fim = value;
            }
        }

        public virtual bool isAberto
        {
            get
            {
                return !_Fim.HasValue;
            }
        }

        public virtual ICollection<Intervalo> Intervalos { get; set; }

        #endregion        

        protected DiaTrabalho() { }

        public DiaTrabalho(DateTime data, TimeSpan inicio, Funcionario funcionario)
        {
            base.checkPreConstructor();
            Check.Require(funcionario != null, "O usuário não deve ser nulo");
            
            base.Data = data;
            base.Funcionario = funcionario;
            this.Inicio = inicio;
            this.Intervalos = new List<Intervalo>();            
        }

        public virtual void registrarIntervalo(TipoIntervalo tipoIntervalo, IDataHoraStrategy dataHoraStrategy)
        {
            if (intervaloFoiRegistrado(tipoIntervalo))
            {
                var intervalo = getIntervalo(tipoIntervalo);
                if (intervalo.Saida.HasValue)
                    throw new IntervaloJaRegistradoException(tipoIntervalo);

                intervalo.Saida = dataHoraStrategy.getDataHoraAtual().TimeOfDay;
            }
            else
            {
                if (algumIntervaloEmAberto())
                    throw new IntervaloEmAbertoException(getIntervaloEmAberto());

                Intervalos.Add(new Intervalo(tipoIntervalo, dataHoraStrategy.getDataHoraAtual().TimeOfDay));
            }
        }

        public virtual Intervalo getIntervalo(TipoIntervalo tipoIntervalo)
        {
            try
            {
                return Intervalos.Single(x => x.TipoIntervalo.Nome.Equals(tipoIntervalo.Nome));
            }
            catch (InvalidOperationException)
            {
                throw new IntervaloNaoRegistradoException(tipoIntervalo);
            }
        }

        public virtual bool intervaloFoiRegistrado(TipoIntervalo tipoIntervalo)
        {
            return Intervalos.Any(x => x.TipoIntervalo.Nome.Equals(tipoIntervalo.Nome));
        }

        public virtual bool algumIntervaloEmAberto()
        {
            return Intervalos.Any(x => x.isAberto);
        }

        public virtual Intervalo getIntervaloEmAberto()
        {
            return Intervalos.FirstOrDefault(x => x.isAberto);
        }

        public override TimeSpan calcularHorasTrabalhadas()
        {
            if (isAberto) throw new DiaEmAbertoException(this);

            var descanso = new TimeSpan(Intervalos.Sum(x => x.Saida.Value.Subtract(x.Entrada).Ticks));
            var trabalhado = Fim.Value.Subtract(Inicio);

            return trabalhado.Subtract(descanso);
        }
    }
}
