using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Timers;    //timer DefinePriceUpdateInSeconds 
                        //A namespace → The System.Timers namespace proves useful.
                        //With a Timer, we can ensure nothing unexpected has happened.
                        //We can also run a periodic update (to do anything).

using System.Timers;    //timer DefinePriceUpdateInSeconds
using static System.Console;


namespace Workspace_Projetos
{
    //esta classe estará sempre pública para que possa ser chamada e usada sempre que precisarmos; assim como será a conexão para todo o restante desenvolvimento das acções pretendidas
    //TimerExample is a static class, meaning it cannot have instance members or fields.
    //We include the System.Timers namespace and see the Elapsed event function.
    //A summary → The Timer class from the System.Timers namespace can be used to run code on an interval.
    //An interval-based validation approach is recommended for important applications.


    public class CryptoAPI 
    {        
        public Mercado _mercado;
        private static System.Timers.Timer aTimer;
        private static int timer = 30;
        private const double minVariation = -0.05;
        private const double maxVariation = 0.05;

        /*Os preços são atualizados a cada n segundos (definidos através do método DefinePriceUpdateInSeconds()), sendo que a variação máxima a cada iteração é de +/- 0.5%;
        A simulação deverá correr o número de vezes equivalente ao tempo passado desde que o método de cotações foi chamado pela última vez.
        Por exemplo, se o método foi chamado há 10 min, e o intervalo definido é de 30 segundos, então será necessário correr a simulação 20 vezes para obter as cotações atuais.*/

        public CryptoAPI(Mercado mercado)
        {
            _mercado = mercado;
            DefinePriceUpdateInSeconds(timer);  
        }


        // Part 1: setup the timer for X seconds.
        // To add the elapsed event handler:
        // ... Type "_timer.Elapsed += " and press tab twice.
        //https://www.dotnetperls.com/timer
        //https://docs.microsoft.com/en-us/dotnet/api/system.timers.timer.elapsed?view=net-6.0

        #region SetTimer
        private void SetTimer(int priceUpdateInSeconds)
        {
            //o construtor do timer é em milisegundos
            //http://www.macoratti.net/20/07/c_timer1.htm

            aTimer = new System.Timers.Timer(Convert.ToDouble(timer * 1000));
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        #endregion

        #region OnTimedEvent
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        { 
           _mercado.ValorCambioCHOW = _mercado.ValorCambioCHOW + RandomNumberBetween(minVariation, maxVariation);
           _mercado.ValorCambioDOCE = _mercado.ValorCambioDOCE + RandomNumberBetween(minVariation, maxVariation);
           _mercado.ValorCambioGALLO = _mercado.ValorCambioGALLO + RandomNumberBetween(minVariation, maxVariation);
           _mercado.ValorCambioTUGA = _mercado.ValorCambioTUGA + RandomNumberBetween(minVariation, maxVariation);
           Save();
        }
        #endregion

        #region RandomNumberBetween
        private decimal RandomNumberBetween(double minValue, double maxValue)
        {
            Random random = new Random();
            var next = random.NextDouble();

            return Convert.ToDecimal(minValue + (next * (maxValue - minValue)));
        }
        #endregion

        #region AddCoin
        //Permite adicionar uma nova criptomoeda no sistema da corretora; 
        
        public void AddCoin(string coin)
        {
           switch (coin)
            {
                case "TUGA":
                    _mercado.TotalTUGA += 1;
                    break;

                case "CHOW":
                    _mercado.TotalCHOW += 1;
                    break;

                case "GALLO":
                    _mercado.TotalGALLO += 1;
                    break;

                case "DOCE":
                    _mercado.TotalDOCE += 1;
                    break;

                default:
                    
                    WriteLine("Moeda Inválida");
                    break;
            }
        }
        #endregion

        #region RemoveCoin
        //Retira criptomoeda do sistema de cotações
        public void RemoveCoin(string coin)
        {
            switch (coin)
            {
                case "TUGA":
                    if(_mercado.TotalTUGA > 0) 
                    {
                        _mercado.TotalTUGA -= 1;
                    }
                    break;

                case "CHOW":
                    if(_mercado.TotalCHOW > 0) 
                    {
                        _mercado.TotalCHOW -= 1;
                    }
                    break;

                case "GALLO":
                    if(_mercado.TotalGALLO > 0) 
                    {
                        _mercado.TotalGALLO -= 1;
                    }
                    break;

                case "DOCE":
                    if(_mercado.TotalDOCE > 0) 
                    {
                        _mercado.TotalDOCE -= 1;
                    }
                    break;

                default:
                    WriteLine("Moeda Inválida");
                    break;
            }
        }
        #endregion

        #region GetCoins
        //devolve lista de todas as moedas geridas pela corretora;
        public string[] GetCoins()
        {
             return new string[] {"CHOW", "DOCE", "GALLO", "TUGA"};
        }
        #endregion

        #region GetPrices
        //Devolve os preços atualizados de todas as moedas registadas;
        public void GetPrices(out decimal[] prices, out string[] coins)
        {
            prices = new decimal[] {_mercado.ValorCambioCHOW, _mercado.ValorCambioDOCE, _mercado.ValorCambioGALLO, _mercado.ValorCambioTUGA};
            coins = GetCoins();
            Save();
        }
        #endregion

        #region DefinePriceUpdateInSeconds
        //Permite definir o intervalo de tempo em segundos que o módulo atualiza a cotação das moedas;
        public void DefinePriceUpdateInSeconds(int seconds)
        {
            timer = seconds;
            SetTimer(seconds);
        }
        #endregion

        #region GetPriceUpdateInSeconds
        //Permite obter o intervalo de tempo (em segundos) em que o módulo calcula novos preços de cotações;
        public int GetPriceUpdateInSeconds()
        {
            return timer;
        }
        #endregion

        #region Save
        //Permite gravar as cotações e moedas geridas, bem como a data do último câmbio. Sempre que o método GetPrices() é chamado, deve ser chamado também o método Save() para assegurar que em caso de falha do sistema, os dados tenham sido persistidos.
        public void Save()
        {
            File.WriteAllText("mercado.txt", $"{_mercado.TotalCHOW};{_mercado.ValorCambioCHOW};{_mercado.TotalDOCE};{_mercado.ValorCambioDOCE};{_mercado.TotalGALLO};{_mercado.ValorCambioGALLO};{_mercado.TotalTUGA};{_mercado.ValorCambioTUGA}");
        }
        #endregion

        #region Read
        //Permite ler as moedas, câmbio e data a que dizem respeito. Sempre que o programa é iniciado, o sistema deverá ver se o ficheiro já existe, e se sim, carregar todos os dados previamente persistidos.
        public void Read()
        {
            string pathFileMercado = "mercado.txt"; //ficheiro txt criado

             if(File.Exists(pathFileMercado))
            {
                string fileContent = File.ReadAllText(pathFileMercado);
                string[] fileContentSplit = fileContent.Split(';');
                int conteudoFicheiroInteiro;
                decimal conteudoFicheiroDecimal;

                if (int.TryParse(fileContentSplit[0], out conteudoFicheiroInteiro)) 
                { 
                    _mercado.TotalCHOW = conteudoFicheiroInteiro; 
                }
                if (decimal.TryParse(fileContentSplit[1], out conteudoFicheiroDecimal)) 
                { 
                    _mercado.ValorCambioCHOW = conteudoFicheiroDecimal; 
                }
                if (int.TryParse(fileContentSplit[2], out conteudoFicheiroInteiro)) 
                { 
                    _mercado.TotalDOCE = conteudoFicheiroInteiro; 
                }
                if (decimal.TryParse(fileContentSplit[3], out conteudoFicheiroDecimal)) 
                { 
                    _mercado.ValorCambioDOCE = conteudoFicheiroDecimal;
                }
                if (int.TryParse(fileContentSplit[4], out conteudoFicheiroInteiro)) 
                { 
                    _mercado.TotalGALLO = conteudoFicheiroInteiro; 
                }
                if (decimal.TryParse(fileContentSplit[5], out conteudoFicheiroDecimal)) 
                { 
                    _mercado.ValorCambioGALLO = conteudoFicheiroDecimal;
                }
                if (int.TryParse(fileContentSplit[6], out conteudoFicheiroInteiro)) 
                { 
                    _mercado.TotalTUGA = conteudoFicheiroInteiro; 
                }
                if (decimal.TryParse(fileContentSplit[7], out conteudoFicheiroDecimal)) 
                { 
                    _mercado.ValorCambioTUGA = conteudoFicheiroDecimal;
                }                
            }
        }
        #endregion
    }
}