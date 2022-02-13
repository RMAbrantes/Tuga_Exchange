using System;
using System.IO;
using System.Text;
using System.Threading;
using static System.Console;

namespace Workspace_Projetos
{
    public class Administrador
    {
        public decimal TotalComissoes { get; set; }

        public Administrador()
        {
            TotalComissoes = 0;
        }

        public void SaveAdministrador()
        {
            //métodos estáticos para gravar um texto em um arquivo
            File.WriteAllText("administrador.txt", $"{TotalComissoes}");
        }

        public void MostraComissoes()
        {
            WriteLine("Ganhou " + decimal.Round(TotalComissoes, 2) + " em comissões");
            Thread.Sleep(5000);
        }
    }
}