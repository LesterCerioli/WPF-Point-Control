using ControlePonto.Domain.ponto.folga;
using ControlePonto.Domain.services.relatorio;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.WPF.window.consulta.folga
{
    public class DiaFolgaDTO : NotifyPropertyChangedBase, IDataErrorInfo
    {
        private const string FUNCIONARIO_NAO_TRABALHOU = "Funcionário não trabalhou neste dia";        

        private bool _isDiaFolga;
        public bool IsDiaFolga
        {
            get { return _isDiaFolga; }
            set
            {
                if (SetField(ref _isDiaFolga, value))
                {
                    if (value)
                        Descricao = "";
                    else
                        Descricao = FUNCIONARIO_NAO_TRABALHOU;
                }
            }
        }

        public DateTime Data { get; private set; }

        public Funcionario Funcionario { get; private set; }

        private string _descricao;
        public string Descricao
        {
            get { return _descricao; }
            set { SetField(ref _descricao, value); }
        }

        public int DescricaoMax
        {
            get
            {
                return DiaFolga.MAX_DESCRICAO_LENGTH;
            }
        }

        public bool IsEnabled
        {
            get
            {
                if (DateTime.Today > Data)
                    return false;
                return !diaExiste;
            }
        }

        public bool IsReadOnly
        {
            get 
            {
                return diaExiste;
            }
        }

        private bool diaExiste;

        public DiaFolgaDTO(DiaRelatorio diaCalendario, Funcionario funcionario)
        {
            this.Data = diaCalendario.Data;
            this.Funcionario = funcionario;
            diaExiste = true;

            switch (diaCalendario.TipoDia)
            {
                case ETipoDiaRelatorio.FOLGA:
                    _isDiaFolga = true;
                    this.Descricao = ((diaCalendario as DiaPonto).PontoDia as DiaFolga).Descricao;
                    break;

                case ETipoDiaRelatorio.TRABALHO:
                    this.Descricao = "Funcionário trabalhou neste dia";
                    break;

                default:
                    this.Descricao = FUNCIONARIO_NAO_TRABALHOU;
                    diaExiste = false;
                    break;
            }
        }

        #region Tratativa de erros

        public string Error
        {
            get { return String.Empty; }
        }

        public string this[string columnName]
        {
            get 
            { 
                if (columnName == "Descricao")
                {
                    if (string.IsNullOrWhiteSpace(Descricao))
                        return "É obrigatório justificar a razão da folga!";
                }
                return String.Empty;
            }
        }

        #endregion
    }
}
