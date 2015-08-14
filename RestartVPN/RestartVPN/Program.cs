using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RestartVPN
{
    class Program
    {
      

        static void Main(string[] args)
        {

            DateTime time = DateTime.Now;
            
           
            System.Console.WriteLine("Start Aplicatie de Resetare VPN - "+time.ToString());
            while (1==1){
            
                Thread.Sleep(20000);
                checkIp();
            }
            //comment pt git

            //commmit 222
           
        }


        
            static void  checkIp() {
                
                DataClasses1DataContext database = new DataClasses1DataContext();
                string ip = "176.9.103.131";
                try
                {
                    ip = VerifyIp();
                    int nrsecwait = 0;


                    while (ip.Contains("176.9.103.131"))//("89.33.106.3"))
                    {
                        
                        Thread.Sleep(1000);
                        nrsecwait++;

                        if (nrsecwait % 10 == 0)
                        {
                            System.Console.WriteLine("VPN neconectat - " + DateTime.Now.ToString());
                            ip = VerifyIp();
                            //insereaza in tabel ip=ul
                            database.Insert_restartVPN_last_ip(ip);
                           
                        }
                            
                        //System.Console.WriteLine(nrsecwait);

                        if (nrsecwait > 30 && nrsecwait % 60 == 0)
                        {


#if DEBUG
                                
                                 System.Console.WriteLine("Se Face Restart La VPN - " + DateTime.Now.ToString());
                               // Process.Start("C:\\Users\\Adrian.Ghibaor\\Documents\\Visual Studio 2013\\Projects\\GetAgentie\\GetAgentie\\bin\\Release\\GetAgentie.exe");
#else
                            DateTime time = DateTime.Now;
                            System.Console.WriteLine("Se Face Restart La VPN - " + DateTime.Now.ToString());
                            Process.Start("script_vpn.exe");
                            Thread.Sleep(25000); // se asteapta 25 secunde pt ca si script_vpn are un sleep de 10 secunde pana face connnect si poate dura 10 secunde pana se conecteaza
                            System.Console.WriteLine("Acum ar trebui sa fie connectat - " + DateTime.Now.ToString());
#endif
                            ip = VerifyIp();
                        }
                    }

                    System.Console.WriteLine("VPN ok");
                    //inseereaza in tabel ip-ul
                    database.Insert_restartVPN_last_ip(ip);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Nu s-a putut verifica ip-ul, reincerc in 20 secunde - " + DateTime.Now.ToString());
                    //inseereaza in tabel ip-ul
                    database.Insert_restartVPN_last_ip(ip);
                }
                

            }

            static string VerifyIp()
            {
                try
                {
                    WebClient wc = new WebClient();
                    //string adi = wc.DownloadString(@"http://www.checkip.com/").Trim();
                    //Match m = Regex.Match(adi, @"<span class=""green"">(?<ip>.*?)</span>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    string adi = wc.DownloadString(@"http://geoip.hidemyass.com/").Trim();
                    Match m = Regex.Match(adi, @"align=absmiddle>(?<ip>.*?)</span>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

                    return m.Groups["ip"].Value.ToString();
                }
                catch (Exception e)
                {

                    throw;
                    
                }
            }


    }
}
