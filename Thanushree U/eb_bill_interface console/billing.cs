using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace eb_bill_interface
{
    public class Billing : IElectricityBill
    {
        public double Calculate(int units)
        {
            double totalBill;
            double rate;
            if (units <= 100)
            {
                rate = 0;
                totalBill = rate * 0;
                return totalBill;

            }
            else if (units >= 101 && units <= 1000)
            {
                rate = 5.0;
                totalBill = (units - 100) * rate;
                return totalBill;
            }
            else if (units >= 1001 && units <= 10000)
            {
                rate = 10.0;
                totalBill = (900 * 5) + ((units - 1000) * rate);
                return totalBill;
            }
            else if (units >= 10001 && units <= 30000)
            {
                rate = 20.0;
                totalBill = (900 * 5) + (9000 * 10) + (units - 20000) * rate;
                return totalBill;
            }
            else
            {
                rate = 35.0;
                totalBill = (900 * 5) + (9000 * 10) + (20000 * 20) + (units * rate);
                return totalBill;
            }
        }

        public double Taxes(char site, int units)
        {
            double Value = 0;
            double totalBill = Calculate(units);
            if (char.ToUpper(site) == 'C')
            {
                Value = totalBill * 10.0;
            }
            else if (char.ToUpper(site) == 'V')
            {
                Value = totalBill * 0;
            }
            else if (char.ToUpper(site) == 'D')
            {
                bool check;
                do
                {
                    Console.Write("\nEnter the type of area (C for City, T for Town, V for Village): \n");
                    char areaType = char.ToUpper(Console.ReadKey().KeyChar);
                    Console.WriteLine();
                    if (char.ToUpper(areaType) == 'C')
                    {
                        Value = totalBill * 5.0;
                        check = true;
                    }
                    else if (char.ToUpper(areaType) == 'T')
                    {
                        Value = totalBill * 2.0;
                        check = true;
                    }
                    else if (char.ToUpper(areaType) == 'V')
                    {
                        Value = totalBill * 0;
                        check = true;
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid area type.Try again");
                        check = false;
                    }
                } while (check == false);
            }
            else
            {
                Console.WriteLine("\nInvalid site type.Try again");
            }
            return Value;
        }
        public void Bill_recipt()
        {
            int units = 0; int Bid = 0;
            double Pay = 0; double Bal = 0;
            string Cname; double amnt = 0; bool check;
            char exit;
            char site;

            Console.WriteLine("\n\n---Please enter the following details for Electricity billing ---\n");
            Console.WriteLine("Customer Name :");
            Cname = Convert.ToString(Console.ReadLine());
            do
            {
                try
                {
                    Console.WriteLine("\nBill ID : ");
                    Bid = Convert.ToInt32(Console.ReadLine());
                    check = true;
                }
                catch (FormatException tc)
                {
                    Console.WriteLine("\nInvalid Input! Try again  " + tc.Message);
                    check = false;
                }
            } while (check == false);
            do
            {
                try
                {
                    Console.Write("\nEnter electricity consumption in units:\n ");
                    units = Convert.ToInt32(Console.ReadLine());
                    check = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nInvalid Input! Try again");
                    check = false;
                }
            } while (check == false);
            do
            {
                Console.Write("\nEnter the type of site.(C for Commercial, V for Village, D for Domestic)\n ");
                 site = char.ToUpper(Console.ReadKey().KeyChar);

                try
                {
                    amnt = Taxes(site, units);
                    Console.WriteLine("\nAmount to be paid -> Rs. {0}", amnt);
                    Console.WriteLine("\nEnter the amount You wish to pay: \n");
                    Pay = Convert.ToInt32(Console.ReadLine());
                    Bal = amnt - Pay;
                    check = true;
                    cnct(units, site, Bid, Cname, Pay, Bal, amnt);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid amount type! Pay again\n");
                    check = false;
                }
            } while (check == false);

            DateTime dt = DateTime.Now;
            Console.WriteLine("\n\n\n\n--Electricity bill For the month of " + dt.ToString("MMMM") + "----\n");
            Console.WriteLine("Customer Name: {0}", Cname);
            Console.WriteLine("Billing ID: {0}", Bid);
            Console.WriteLine("Units Consumed: {0}", units);
            Console.WriteLine("Area type {0}", site);
            Console.WriteLine("Amount to be paid -> Rs. {0}", amnt);
            Console.WriteLine("Amount Paid -> Rs.{0}", Pay);
            Console.WriteLine("Balance Amount to be paid is Rs. {0}", Bal);
            Console.Write("\nEnter 'Y' to generate another bill\nOtherwise any other key to exit\n\n\n ");
            exit = char.ToUpper(Console.ReadKey().KeyChar);
        }
           
        
        public void cnct(int units, char site, int Bid, string Cname, double Pay, double Bal, double amnt)
        {
            string connectionString = "Data Source=DESKTOP-F3CK6RQ;Initial Catalog=Electricty_Bill;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            //string query = "INSERT INTO eb_bill (Bid,Cname,units,amnt,Pay,Bal)" + "VALUES('" + Bid + "','" + Cname + "','" + units + "','" + amnt + "','" + Pay + "','" + Bal + "')";
            string query = $"INSERT INTO eb_bill (Bid,Cname,units,amnt,Pay,Bal) VALUES({Bid},'{Cname}',{units},{amnt},{Pay},{Bal})";
            SqlCommand insertcmd = new SqlCommand(query, connection);
            insertcmd.ExecuteNonQuery();
            connection.Close();
        }
    }
    }

