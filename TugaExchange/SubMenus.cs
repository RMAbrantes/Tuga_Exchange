using System;
using System.IO;
using System.Text;
using System.Threading;
using static System.Console;

namespace Workspace_Projetos
{
    public static class SubMenus
    {
        #region MenuInvestidor()
        public static bool MenuInvestidor(Investidor investidor, Administrador administrador, CryptoAPI simulacao)
        {
            Clear();
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("MENU INVESTIDOR:\n");
            WriteLine("");
            ForegroundColor = ConsoleColor.DarkGray;
            WriteLine("1. Depositar.");
            WriteLine("2. Comprar Moeda.");
            WriteLine("3. Vender Moeda.");
            WriteLine("4. Mostrar Portfólio.");
            WriteLine("5. Mostrar Câmbio.");
            WriteLine("6. Sair.");
            Write("\r\nSelecione uma opção: ");

            switch (ReadLine())
            {
                case "1":
                    decimal totalDepositado = investidor.Depositar();
                    OutputEncoding = Encoding.UTF8; //Mostra símbolo de €
                    WriteLine($"Depositou {totalDepositado}€.\nTem agora: " + Decimal.Round(investidor.EurosDepositados, 2) + $"€");
                    
                    Thread.Sleep(5000);
                    return true;

                case "2":
                    int totalComprado = investidor.ComprarMoeda(simulacao, administrador);
                    WriteLine($"Comprou {totalComprado}.\nTem agora: " + Decimal.Round(investidor.EurosDepositados, 2));

                    Thread.Sleep(5000);
                    return true;

                case "3":
                    int totalVendido = investidor.VenderMoeda(simulacao, administrador);
                    WriteLine($"Vendeu {totalVendido}.\nTem agora: " + Decimal.Round(investidor.EurosDepositados, 2));

                    Thread.Sleep(5000);
                    return true;

                case "4":
                    //O portfólio representa os vários ativos do investidor.
                    //(Arredondar sempre a duas casas decimais e apresentar as colunas alinhadas)
                    //Exemplo:
                    //100 EUR @ 1.00 | 100.00 EUR
                    //99 CHOW @ 1.20 | 118.80 EUR

                    WriteLine($"Dinheiro em caixa: " + Decimal.Round(investidor.EurosDepositados, 2));
                    decimal[] precos;
                    string[] moedas;
                    simulacao.GetPrices(out precos, out moedas);

                    ForegroundColor = ConsoleColor.DarkCyan;
                    WriteLine("Moedas" + "\t" + "Valor cambio" + "\t" + "Total");
                    WriteLine($"{investidor.TotalCHOW} CHOW" + "\t" + "@ " + Decimal.Round(precos[0], 2) + "\t" + "\t" + Decimal.Round(investidor.TotalCHOW * precos[0], 2));
                    WriteLine($"{investidor.TotalDOCE} DOCE" + "\t" + "@ " + Decimal.Round(precos[1], 2) + "\t" + "\t" + Decimal.Round(investidor.TotalDOCE * precos[1], 2));
                    WriteLine($"{investidor.TotalGALLO} GALLO" + "\t" + "@ " + Decimal.Round(precos[2], 2) + "\t" + "\t" + Decimal.Round(investidor.TotalGALLO * precos[2], 2));
                    WriteLine($"{investidor.TotalGALLO} GALO" + "\t" + "@ " + Decimal.Round(precos[2], 2) + "\t" + "\t" + Decimal.Round(investidor.TotalGALLO * precos[2], 2));
                    WriteLine($"{investidor.TotalTUGA} TUGA" + "\t" + "@ " + Decimal.Round(precos[3], 2) + "\t" + "\t" + Decimal.Round(investidor.TotalTUGA * precos[3], 2));

                    Thread.Sleep(5000);
                    return true;

                case "5":
                    //A opção de visualização do câmbio mostra todas as moedas registadas seguidas do câmbio atual.
                    //Exemplo:
                    //CHOW 0.95
                    //DOCE 1.23
                    //GALO 2.03
                    //TUGA 1.15

                    simulacao.GetPrices(out precos, out moedas);
                    for (int i = 0; i <= 3; i++)
                    {
                        WriteLine(moedas[i] + "\t" + Decimal.Round(precos[i], 2));
                    }
                    Thread.Sleep(5000);

                    return true;
                case "6":
                    return false;
                default:
                    return true;
            }
        }
        #endregion

        #region MenuAdministrador        
        public static bool MenuAdministrador(Administrador administrador, CryptoAPI simulacao)
        {
            Clear();
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("MENU ADMINISTRADOR:\n");
            WriteLine("");
            ForegroundColor = ConsoleColor.DarkGray;
            WriteLine("1. Adicionar Moeda.");
            WriteLine("2. Remover Moeda.");
            WriteLine("3. Ver Relatório de Comissões.");
            WriteLine("4. Sair.");
            Write("\r\nSelecione uma opção: ");

            switch (ReadLine())
            {
                case "1":
                    Clear();
                    simulacao.AddCoin(MenuTipoMoeda());
                    return true;
                case "2":
                    Clear();
                    simulacao.RemoveCoin(MenuTipoMoeda());
                    return true;
                case "3":
                    Clear();
                    administrador.MostraComissoes();
                    return true;
                case "4":
                    return false;
                default:
                    return true;
            }
        }
        #endregion

        #region Moeda MenuTipoMoeda
        public static string MenuTipoMoeda()
        {

            Clear();
            WriteLine("ESCOLHA A MOEDA:\n");
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine("1. TUGA.");
            ForegroundColor = ConsoleColor.DarkRed;
            WriteLine("2. CHOW.");
            ForegroundColor = ConsoleColor.DarkCyan;
            WriteLine("3. GALLO.");
            ForegroundColor = ConsoleColor.Magenta;
            WriteLine("4. DOCE.");
            ForegroundColor = ConsoleColor.White;
            Write("\r\nSelecione uma opção: ");

            switch (ReadLine())
            {
                case "1":
                    return "TUGA";
                case "2":
                    return "CHOW";
                case "3":
                    return "GALLO";
                case "4":
                    return "DOCE";
                default:
                    WriteLine("Escolha uma opção válida.");
                    return "";
            }
        }
        #endregion
    }
}
