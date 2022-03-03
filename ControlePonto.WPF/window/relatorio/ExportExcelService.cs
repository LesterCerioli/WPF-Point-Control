using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.relatorio;
using ControlePonto.Infrastructure.utils;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.WPF.window.relatorio
{
    public class ExportExcelService
    {
        private List<DiaRelatorioViewModel> dias;
        private ITipoIntervaloRepository tipoIntervaloRepository;
        private TipoIntervalo tipoAlmoco;
        private RelatorioPonto relatorio;

        public ExportExcelService(RelatorioPonto relatorio, List<DiaRelatorioViewModel> dias, ITipoIntervaloRepository tipoIntervaloRepository)
        {
            this.relatorio = relatorio;
            this.dias = dias;
            this.tipoIntervaloRepository = tipoIntervaloRepository;
            this.tipoAlmoco = tipoIntervaloRepository.findByName("ALMOÇO");
        }

        public ExcelPackage Exportar(string filename)
        {
            FileInfo newFile = new FileInfo(filename);
            var excel = new ExcelPackage(newFile);
            var ws = excel.Workbook.Worksheets.Add(relatorio.Funcionario.Nome);

            WriteTitle(ws);
            WriterHeaders(ws);

            var cell = new Cell('A', 3);
            foreach (var dia in dias)
            {
                WriteRow(ws, dia, cell);

                cell.Column = 'A';
                cell.nextLine();
            }

            excel.Save();
            return excel;
        }

        private string[] GetHeaders()
        {
            return new string[] {
                "Data", "Descrição",
                "Entrada", "Saída almoço", "Retorno almoço", "Saída",
                "Horas trabalhadas", "Horas devedoras", "Extras", "Extras 100%"
            };
        }

        private void WriteTitle(ExcelWorksheet ws)
        {
            var range = "A1:J1";
            ws.Cells["A1"].Value = $"{relatorio.Funcionario.Nome} ({relatorio.PeriodoInicio.ToShortDateString()} - {relatorio.PeriodoFim.ToShortDateString()})";

            ws.Cells[range].Merge = true;
            ws.Cells[range].Style.Font.Bold = true;
            ws.Cells[range].Style.Font.Size = 21;
        }

        private void WriterHeaders(ExcelWorksheet ws)
        {
            var cell = new Cell('A', 2);
            foreach (string header in GetHeaders())
            {
                ws.Cells[cell.format()].Value = header;
                cell.nextColumn();
            }

            var range = $"A1:{cell.format()}";
            ws.Cells[range].Style.Font.Bold = true;
            ws.Cells[range].Style.Font.Size = 14;
        }

        public void WriteRow(ExcelWorksheet ws, DiaRelatorioViewModel dia, Cell cell)
        {
            ws.Cells[cell.format()].Value = dia.Data;

            cell.nextColumn();
            ws.Cells[cell.format()].Value = dia.Info;            

            if (!WriteWorkDay(ws, dia, cell))
            {
                for (int i = 0; i < 4; i++) //Skip cells
                    cell.nextColumn();
            }

            cell.nextColumn();
            WriteTimeCell(ws, cell, dia.HorasTrabalhadas);

            cell.nextColumn();
            WriteTimeCell(ws, cell, dia.HorasDevedoras);

            cell.nextColumn();
            WriteTimeCell(ws, cell, dia.HorasExtras);

            cell.nextColumn();
            WriteTimeCell(ws, cell, dia.HorasExtras100);
        }

        private bool WriteWorkDay(ExcelWorksheet ws, DiaRelatorioViewModel dia, Cell cell)
        {
            if (dia.DiaRelatorio is IDiaComPonto)
            {
                var diaComPonto = (dia.DiaRelatorio as IDiaComPonto);
                if (diaComPonto.PontoDia is DiaTrabalho)
                {
                    var diaTrabalho = (diaComPonto.PontoDia as DiaTrabalho);

                    cell.nextColumn();
                    WriteTimeCell(ws, cell, diaTrabalho.Inicio.ToString());

                    if (! WriteInterval(ws, diaTrabalho, cell))
                    {
                        cell.nextColumn();//Skip
                        cell.nextColumn();
                    }

                    cell.nextColumn();
                    WriteTimeCell(ws, cell, diaTrabalho.Fim.ToString());

                    return true;
                }
            }

            return false;
        }

        private void WriteTimeCell(ExcelWorksheet ws, Cell cell, string time)
        {
            ws.Cells[cell.format()].Style.Numberformat.Format = "hh:mm:ss";
            if (!string.IsNullOrEmpty(time))
                ws.Cells[cell.format()].Value = TimeSpan.Parse(time);
        }

        private bool WriteInterval(ExcelWorksheet ws, DiaTrabalho dia, Cell cell)
        {
            try
            {
                var almoco = dia.getIntervalo(tipoAlmoco);
                cell.nextColumn();
                WriteTimeCell(ws, cell, almoco.Entrada.ToString());

                cell.nextColumn();
                WriteTimeCell(ws, cell, almoco.Saida.ToString());

                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }

    public class Cell
    {
        public char Column { get; set; }
        public int Line { get; set; }

        public Cell(char col, int lin)
        {
            this.Column = col;
            this.Line = lin;
        }

        public void nextColumn()
        {
            Column++;
        }

        public void nextLine()
        {
            Line++;
        }

        public string format()
        {
            return $"{Column}{Line}";
        }
    }
}
