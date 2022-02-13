using System;
using System.IO;
using System.Text;
using static System.Console;

namespace Workspace_Projetos
{
    //A classe Investidor tem as seguintes propriedades:
    public class Investidor
    {
        public decimal EurosDepositados { get; set; }
        public int TotalCHOW { get; set; }
        public int TotalDOCE { get; set; }
        public int TotalGALLO { get; set; }
        public int TotalTUGA { get; set; }

        public Investidor()
        {
            EurosDepositados = 0;
            TotalCHOW = 0;
            TotalDOCE = 0;
            TotalGALLO = 0;
            TotalTUGA = 0;
        }

        public void SaveInvestidor()
        {
            //métodos estáticos para gravar um texto em um arquivo; abre e fecha o arquivo
            File.WriteAllText("investidor.txt", $"{EurosDepositados};{TotalCHOW};{TotalDOCE};{TotalGALLO};{TotalTUGA}");
        }

        #region Depositar
        //a conta inicial está a 0 → fazer depósito em €
        public decimal Depositar()
        {
            decimal totalDepositar;

            OutputEncoding = Encoding.UTF8;
            WriteLine($"\r\nSelecione o montante a depositar, em Euro/€.");
            if (!decimal.TryParse(ReadLine(), out totalDepositar))
            {
                WriteLine("Por favor insira um valor válido.");
            }

            //this é uma palavra reservada; refª ao próprio objecto → permite saber que se trata de um atributo 
            this.EurosDepositados += totalDepositar;
            SaveInvestidor();

            return totalDepositar;
        }
        #endregion

        #region ComprarMoeda
        public int ComprarMoeda(CryptoAPI simulacao, Administrador administrador)
        {
            int totalComprar;

            string tipomoedaSelecionada = SubMenus.MenuTipoMoeda();
            if(tipomoedaSelecionada == "") return 0;

            WriteLine($"\r\nSelecione o montante a comprar de {tipomoedaSelecionada}(s).");
            if(!int.TryParse(ReadLine(), out totalComprar))
            {
                WriteLine($"Por favor insira unidades válidas de {tipomoedaSelecionada}(s) a comprar");
            }

            //TODO
            ////validar se há suficiente numero de moedas no mercado e descontar valores em caixa consoante o preço na simulação
            
            decimal valorDescontar = 0;
            switch(tipomoedaSelecionada)
            {
                 case "CHOW":
                    if(totalComprar > simulacao._mercado.TotalCHOW) 
                    { 
                        WriteLine($"Não há CHOW's suficientes ({simulacao._mercado.TotalCHOW} existentes)");
                        return 0;
                    }
                    if(totalComprar * simulacao._mercado.ValorCambioCHOW > this.EurosDepositados) 
                    { 
                        WriteLine($"Não há diheiro para comprar {totalComprar} CHOW's"); 
                        return 0;
                    }
                    
                    this.TotalCHOW += totalComprar;
                    valorDescontar = totalComprar * simulacao._mercado.ValorCambioCHOW;
                    
                    simulacao._mercado.TotalCHOW -= totalComprar;                    
                    break;

                case "DOCE":
                    if(totalComprar > simulacao._mercado.TotalDOCE) 
                    { 
                        WriteLine($"Não há DOCE's suficientes ({simulacao._mercado.TotalDOCE} existentes)"); 
                        return 0;
                    }
                    if(totalComprar * simulacao._mercado.ValorCambioDOCE > this.EurosDepositados) 
                    { 
                        WriteLine($"Não há diheiro para comprar {totalComprar} DOCE's"); 
                        return 0;
                    }
                    
                    this.TotalDOCE += totalComprar;
                    valorDescontar = totalComprar * simulacao._mercado.ValorCambioDOCE;
                    
                    simulacao._mercado.TotalDOCE -= totalComprar;                    
                    break;

                case "GALLO":
                    if(totalComprar > simulacao._mercado.TotalGALLO)
                    { 
                        WriteLine($"Não há GALO's suficientes ({simulacao._mercado.TotalGALLO} existentes)");
                        return 0;
                    }
                    if(totalComprar * simulacao._mercado.ValorCambioGALLO > this.EurosDepositados) 
                    { 
                        WriteLine($"Não há diheiro para comprar {totalComprar} GALO's"); 
                        return 0;
                    }
                                        
                    this.TotalGALLO += totalComprar;
                    valorDescontar = totalComprar * simulacao._mercado.ValorCambioGALLO;

                    simulacao._mercado.TotalGALLO -= totalComprar;                    
                    break;

                case "TUGA":
                    if(totalComprar > simulacao._mercado.TotalTUGA)
                    { 
                        WriteLine($"Não há TUGA's suficientes ({simulacao._mercado.TotalTUGA} existentes)"); 
                        return 0;
                    }
                    if(totalComprar * simulacao._mercado.ValorCambioTUGA > this.EurosDepositados) 
                    { 
                        WriteLine($"Não há diheiro para comprar {totalComprar} TUGA's"); 
                        return 0;
                    }
                    
                    this.TotalTUGA += totalComprar;
                    valorDescontar = totalComprar * simulacao._mercado.ValorCambioTUGA;

                    simulacao._mercado.TotalTUGA -= totalComprar;                    
                    break;
            }

            //calcular comissao
            decimal totalComissao = Convert.ToDecimal(valorDescontar * 1/100);
            administrador.TotalComissoes += totalComissao;

            //descontar o dinheiro calculado no switch
            this.EurosDepositados -= (valorDescontar + totalComissao);
            SaveInvestidor();
            return totalComprar;
        }
        #endregion

        #region VenderMoeda        
        public int VenderMoeda(CryptoAPI simulacao, Administrador administrador) 
        {
            int totalVender;

            string tipomoedaSelecionada = SubMenus.MenuTipoMoeda();
            if(tipomoedaSelecionada == "") return 0;

            WriteLine($"\r\nSelecione o montante a vender de {tipomoedaSelecionada}(s).");
            if(!int.TryParse(ReadLine(), out totalVender))
            {
                WriteLine($"Por favor insira unidades válidas de {tipomoedaSelecionada}(s) a vender");
            }

            decimal valorAcrescentar = 0;
            switch(tipomoedaSelecionada)
            {
                 case "CHOW":
                    if(totalVender > this.TotalCHOW) 
                    { 
                        WriteLine("Não há assim tantos CHOW's para vender"); 
                        return 0;
                    }
                    
                    this.TotalCHOW -= totalVender;
                    valorAcrescentar = totalVender * simulacao._mercado.ValorCambioCHOW;
                    
                    simulacao._mercado.TotalCHOW += totalVender;                    
                    break;

                case "DOCE":
                    if(totalVender > this.TotalDOCE) 
                    { 
                        WriteLine("Não há assim tantos DOCE's para vender"); 
                        return 0;
                    }
                    
                    this.TotalDOCE -= totalVender;
                    valorAcrescentar = totalVender * simulacao._mercado.ValorCambioDOCE;

                    simulacao._mercado.TotalDOCE += totalVender;                    
                    break;

                case "GALLO":
                    if(totalVender > this.TotalGALLO) 
                    { 
                        WriteLine("Não há assim tantos GALLO's para vender"); 
                        return 0;
                    }
                    
                    this.TotalGALLO -= totalVender;
                    valorAcrescentar = totalVender * simulacao._mercado.ValorCambioGALLO;

                    simulacao._mercado.TotalGALLO += totalVender;                    
                    break;

                case "TUGA":
                    if(totalVender > this.TotalTUGA) 
                    { 
                        WriteLine("Não há assim tantos TUGA's para vender"); 
                        return 0;
                    }
                    
                    this.TotalTUGA -= totalVender;
                    valorAcrescentar = totalVender * simulacao._mercado.ValorCambioTUGA;

                    simulacao._mercado.TotalTUGA += totalVender;                    
                    break;
            }

             //calcular comissao
            decimal totalComissao = Convert.ToDecimal(valorAcrescentar * 1/100);
            administrador.TotalComissoes += totalComissao;
                        
            //contabilizar o dinehiro ganho no switch menos a comissão
            this.EurosDepositados += (valorAcrescentar - totalComissao);

            SaveInvestidor();
            return totalVender;
        }
        #endregion        
    }
}