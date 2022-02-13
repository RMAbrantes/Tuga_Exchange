using System;
using System.IO;
using System.Text;
using static System.Console;

namespace Workspace_Projetos
{
    public class Mercado
    {
        //Valor das Moedas tendo em conta o câmbio;

        //Get → propriedade de leitura = obter um valor de uma propriedade
        //Set → somente de escrita = atribuir valor a uma propriedade
        //Get Set = readonly + write only

        public int TotalCHOW { get; set; }
        public decimal ValorCambioCHOW { get; set; }
        public int TotalDOCE { get; set; }
        public decimal ValorCambioDOCE { get; set; }
        public int TotalGALLO { get; set; }
        public decimal ValorCambioGALLO { get; set; }
        public int TotalTUGA { get; set; }
        public decimal ValorCambioTUGA { get; set; }

        public Mercado()
        {
            TotalCHOW = 0;
            ValorCambioCHOW = 1;
            TotalDOCE = 0;
            ValorCambioDOCE = 1;
            TotalGALLO = 0;
            ValorCambioGALLO = 1;
            TotalTUGA = 0;
            ValorCambioTUGA = 1;
        }
       
    }
}