using System;
using System.IO;    //Inheritance Objects → File
using System.Text;  //para usarmos File //https://docs.microsoft.com/en-us/dotnet/api/
using static System.Console;

namespace Workspace_Projetos
{
    class Program
    {
        //é necessário declarar os objectos para de seguida inicializá-los
        private static readonly Investidor _investidor = new Investidor();
        private static readonly Administrador _administrador = new Administrador();
        private static readonly Mercado _mercado = new Mercado();
                
        static void Main(string[] args)
        {
            //The file to open for reading. (path); returns a string containing all text in the file
            //A classe Path efetua operações com strings que representam informação sobre ficheiros e diretorias.
            string pathFileInvestidor = "investidor.txt"; //ficheiro txt criado
            string pathFileAdministrador = "administrador.txt"; //ficheiro txt criado

            if(File.Exists(pathFileInvestidor))
            {
                string fileContent = File.ReadAllText(pathFileInvestidor);
                string[] fileContentSplit = fileContent.Split(';');
                int conteudoFicheiroInteiro;
                decimal conteudoFicheiroDecimal;

                if (decimal.TryParse(fileContentSplit[0], out conteudoFicheiroDecimal)) { _investidor.EurosDepositados = conteudoFicheiroDecimal; }
                if (int.TryParse(fileContentSplit[1], out conteudoFicheiroInteiro)) { _investidor.TotalCHOW = conteudoFicheiroInteiro; }
                if (int.TryParse(fileContentSplit[2], out conteudoFicheiroInteiro)) { _investidor.TotalDOCE = conteudoFicheiroInteiro; }
                if (int.TryParse(fileContentSplit[3], out conteudoFicheiroInteiro)) { _investidor.TotalGALLO = conteudoFicheiroInteiro; }
                if (int.TryParse(fileContentSplit[4], out conteudoFicheiroInteiro)) { _investidor.TotalTUGA = conteudoFicheiroInteiro; }
            }

            if(File.Exists(pathFileAdministrador))
            {
                string fileContent = File.ReadAllText(pathFileAdministrador);
                decimal valueFromFile;
                if (decimal.TryParse(fileContent, out valueFromFile))
                {
                    _administrador.TotalComissoes = valueFromFile;
                }
            }   

            CryptoAPI simulacao = new CryptoAPI(_mercado);
            simulacao.Read();

            //TODO iniciar a simulação
            /*
            verificar se já há ficheiro de cambios
            verificar se já há ficheiro com informação do investidor
            Acho q o Admin tb terá q ter um fich com valores das comissões

            Não havendo info persistida a simulação deverá começar com:
            Investidor - 0 em caixa e 0 moedas
            Admin - 0 comissões
            Mercado - todas as moedas deverão iniciar com cotação 1 EUR
                    - nenhuma moeda no mercado pois o Admin é que adiciona e remove moedas
            
            de n em n segundos sao atualizados os preços com uma iteração maxima de +/-0.5%
             */


            //#region - visa diminuir visualmente o código e deixá-lo agrupado por tipo de execução, por exemplo: Eventos, Funções e Métodos, Construtores, Enumeradores, Variáveis, etc. Para programas que possui inúmeras linhas de códigos dentro de classes, este recurso ajuda a melhor dividir o código. Deve começar com #region e terminar com #endregion
            

            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu(simulacao);
            }
            _investidor.SaveInvestidor();
            _administrador.SaveAdministrador();
            simulacao.Save();
        }

        #region MainMenu()
        public static bool MainMenu(CryptoAPI simulacao)
        {
            bool showMenuInvestidor = false;
            bool showMenuAdministrador = false; 
            Clear();
            Title = "TUGA EXCHANGE";    //muda o nome do título da consola
            ForegroundColor = ConsoleColor.DarkGreen;
            WriteLine($"\r\nBem-vindo à {Title}!");
            WriteLine("");
            ForegroundColor = ConsoleColor.DarkGray;
            WriteLine("A. Sou Investidor.");
            WriteLine("B. Sou Administrador.");
            WriteLine("C. Sair.");
            Write("\r\nSelecione uma opção: ");
        
            switch (ReadLine())
            {
                case "A":
                    Clear();
                    showMenuInvestidor = true;
                    while(showMenuInvestidor){
                        showMenuInvestidor = SubMenus.MenuInvestidor(_investidor, _administrador, simulacao);
                    }
                    return true;
                case "B":
                    Clear();
                    showMenuAdministrador = true;
                    while(showMenuAdministrador){
                        showMenuAdministrador = SubMenus.MenuAdministrador(_administrador, simulacao);
                    }
                    return true;
                case "C":
                    simulacao.Save();
                    return false;
                default:
                    return true;
            }
        }
        #endregion
    }    
}
