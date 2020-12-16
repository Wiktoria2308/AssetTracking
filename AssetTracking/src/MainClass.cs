using AssetTracking.src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AssetTracking
{

    //this class which has all necessary methods to run the program 
    public class MainClass
    {
        static AssetContext context = new AssetContext();
        string number;
        string number2;
        string brand;
        string model;
        string price;
        string purchase;
        string office;
        string chooseCountry;

        /* Main method which starts the program. It is using do while loop and switch statements.  
         * It is working until user will input 'q' as an end of the program. 
         * By using 1 user can add a new item to the list, computer or phone. By using 2, user can see list of items sorted 
         * by purchase date or by office , or computer first. 
         * 
         */
        public void start()
        {
            do
            {
                // Ask the user to choose an option.
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\nMenu:\n\n1.Add new product\n2.Delate a product\n3.Update a product\n4.Show list of products\n(Click q to exit.)\n");
                number = Console.ReadLine();
                string exitCheck = number;
                exitCheck = exitCheck.ToLower();
                exitCheck = exitCheck.Trim();
                number = number.Trim();
                switch (number)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("\n1. Computer\n2. Phone");
                        number2 = Console.ReadLine();
                        if (number2 == "1" || number2 == "2")
                        { AddNewProduct(number2); }
                        else
                        { Console.WriteLine("Incorrect input. Try again"); }
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("\nWhat kind of product do you want to remove\n1.Computer\n2.Phone");
                        int choice = int.Parse(Console.ReadLine());
                        if (choice == 1 || choice == 2)
                        { RemoveProduct(choice); }
                        else
                        { Console.WriteLine("Incorrect input. Try again"); }
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("\nWhat kind of product do you want to update?\n1. Computer\n2. Phone");
                        int choice2 = int.Parse(Console.ReadLine());
                        if (choice2 == 1 || choice2 == 2)
                        { UpdateProduct(choice2); }
                        else
                        { Console.WriteLine("Incorrect input. Try again"); }
                        
                        break;
                    case "4":
                        SortedListComputerFirst();
                        SortedListByOffice();
                        SortListByPurchaseDate();
                        break;
                }
                if (exitCheck == "q")
                {
                    Environment.Exit(0);
                }
            }
            while (number != "q");
            Console.ReadLine();
        }
        /************************************END OF START() METHOD *******************************************/


        /************************************************************************OTHER METHODS************************************************************************/

        /*
         * This method is updating a product.
         * It takes one parameter with user choice of product. 
         * If 1 is a computer, if 2 is a phone. 
         * Then another method shows all computers or all phones and user decide which one to change by writing product id.
         * Then id is send to another method. 
         */
        private void UpdateProduct(int choice2)
        {
            if (choice2 == 1)
            {
                if (context.Computers.Any())
                {
                    ShowAllComputers();
                    Console.WriteLine("\nWhich computer do you want to update? ");
                    int computerID = int.Parse(Console.ReadLine());
                    UpdateComputer(computerID);
                }
                else
                {
                    Console.WriteLine("No computers added.");
                }
            }
            else
            {
                if (context.Phones.Any())
                {
                    ShowAllPhones();
                    Console.WriteLine("\nWhich phone do you want to update? ");
                    int phoneID = int.Parse(Console.ReadLine());
                    UpdatePhone(phoneID);
                }
                else
                {
                    Console.WriteLine("No phones added.");
                }
            }
           
        }

        /*
         * This method is removing a product. 
         * It takes one parameter, user choice of product: 1 is a computer and 2 is a phone.
         * Then another method is showing all products and user selects product id.
         * Selected product is removed from database.
         */
        private void RemoveProduct(int choice)
        {
            if (choice == 1)
            {
                if (context.Computers.Any())
                {
                    ShowAllComputers();
                    Console.WriteLine("\nWhich computer do you want to remove? ");
                    int computerID = int.Parse(Console.ReadLine());
                    var computer = context.Computers.Find(computerID);
                    if (computer != null)
                    {
                        context.Computers.Remove(computer);
                        Console.WriteLine("Computer removed");
                    }
                    else { Console.WriteLine($"There is no computer with the id: {computerID} "); }
                }
                else { Console.WriteLine("There is no computers in database."); }
            }
            else
            {
                if (context.Phones.Any())
                {
                    ShowAllPhones();
                    Console.WriteLine("\nWhich phone do you want to remove? ");
                    int phoneID = int.Parse(Console.ReadLine());
                    var phone = context.Phones.Find(phoneID);
                    if(phone != null)
                    {
                        context.Phones.Remove(phone);
                        Console.WriteLine("Phone removed");
                    }
                  else { Console.WriteLine($"There is no computer with the id {phoneID} "); }
                }
                else { Console.WriteLine("There is no phones in database."); }
            }
            
            context.SaveChanges();
        }

        /*This method is adding a new product.
         * It asks user for required data to add a product.
         * Then it checks what kind of product user want to add: 1 is computer and 2 is phone.
         * Then it adding product to database.
         * There is also a method to select a country.
         */
        private void AddNewProduct(string number)
        {
           
          Console.WriteLine("Write product brand name:");
          brand = Console.ReadLine();
          Console.WriteLine("Write product model name:");
          model = Console.ReadLine();

            bool CorrectPrice = true;
            bool CorrectDate = true;
            Console.WriteLine("Write product price (used currency is USD):");
            price = Console.ReadLine();
            try
            {
                decimal v = Convert.ToDecimal(price);
            }
            catch
            {
                Console.WriteLine("Inccorect price format. You can use only numbers. Try again.");
                CorrectPrice = false;
                CorrectDate = false;
            }
            if (CorrectDate == true)
            {
                Console.WriteLine("Write product purchase date in correct format(yyyy.mm.dd):");
                purchase = Console.ReadLine();
                try
                {
                    DateTime dateTime = Convert.ToDateTime(purchase);
                }
                catch
                {
                    Console.WriteLine("Incorrect date format. Try again.");
                    CorrectDate = false;
                }
            }

            if (CorrectPrice && CorrectDate)
            {
                office = SelectCountry();

                if (number == "1")
                {
                    Computer computer = new Computer
                    {
                        ModelName = model,
                        PurchaseDate = purchase,
                        Price = price,
                        BrandName = brand,
                        Office = office,
                    };
                    context.Computers.Add(computer);
                    Console.WriteLine("Computer added.");
                }
                else
                {
                    Phone phone = new Phone
                    {
                        ModelName = model,
                        PurchaseDate = purchase,
                        Price = price,
                        BrandName = brand,
                        Office = office,
                    };
                    context.Phones.Add(phone);
                    Console.WriteLine("Phone added.");
                }
                context.SaveChanges();
            }
        }

        /*This method is selecting a country where the office of the product is.
         * There are three countries which user can select.
         * Then it returns selected country.
         * 
         */
        private string SelectCountry()
        {
            string office = "";
            Console.WriteLine("Choose computer office:\n1. Sweden.\n2. Poland\n3. Canada ");
            chooseCountry = Console.ReadLine();
            switch (chooseCountry)
            {
                case "1":
                    office = "Sweden";
                    break;
                case "2":
                    office = "Poland";
                    break;
                case "3":
                    office = "Canada";
                    break;
                default:
                    Console.WriteLine("Incorrect input. Try again.");
                    break;
            }
            return office;
        }

        /*This method is updating a phone. It asks what user want to change.
         * Add then change it and and makes changes in database.
         */
        private void UpdatePhone(int id)
        {
            var phone = context.Phones.Find(id);
            if (phone == null)
            { Console.WriteLine("There is not such phone in database."); }
            else
            {
                Console.WriteLine("What do you want to change:\n1.Brand name\n2.Model name\n3.Price\n4.Purchase Date\n5.Office\n");
                string number = Console.ReadLine();
                switch (number)
                {
                    case "1":
                        Console.WriteLine("Write new brand name.");
                        string brand = Console.ReadLine();
                        phone.BrandName = brand;
                        break;
                    case "2":
                        Console.WriteLine("Write new model name.");
                        string model = Console.ReadLine();
                        phone.ModelName = model;
                        break;
                    case "3":
                        Console.WriteLine($"Current price is {phone.Price} USD.");
                        Console.WriteLine("Write new price.");
                        string price = Console.ReadLine();
                        phone.Price = price;
                        break;
                    case "4":
                        Console.WriteLine("Write new purchase date.");
                        string date = Console.ReadLine();
                        phone.PurchaseDate = date;
                        break;
                    case "5":
                        Console.WriteLine("Write new office.");
                        string office = Console.ReadLine();
                        phone.Office = office;
                        break;
                    default:
                        Console.WriteLine("Incorrect input. Try again.");
                        break;
                }
                context.SaveChanges();
                Console.WriteLine("Phone updated");
            }
        }

        /*This method is updating a computer. It asks what user want to change.
       * Add then change it and and makes changes in database.
       */
        private void UpdateComputer(int id)
        {
            var computer = context.Computers.Find(id);
            if (computer == null)
            { Console.WriteLine("There is not such computer in database."); }
            else
            {
                Console.WriteLine("What do you want to change:\n1.Brand name\n2.Model name\n3.Price\n4.Purchase Date\n5.Office\n");
                string number = Console.ReadLine();
                switch (number)
                {
                    case "1":
                        Console.WriteLine("Write new brand name.");
                        string brand = Console.ReadLine();
                        computer.BrandName = brand;
                        break;
                    case "2":
                        Console.WriteLine("Write new model name.");
                        string model = Console.ReadLine();
                        computer.ModelName = model;
                        break;
                    case "3":
                        Console.WriteLine($"Current price is {computer.Price} USD.");
                        Console.WriteLine("Write new price.");
                        string price = Console.ReadLine();
                        computer.Price = price;
                        break;
                    case "4":
                        Console.WriteLine("Write new purchase date.");
                        string date = Console.ReadLine();
                        computer.PurchaseDate = date;
                        break;
                    case "5":
                        Console.WriteLine("Write new office.");
                        string office = Console.ReadLine();
                        computer.Office = office;
                        break;
                    default:
                        Console.WriteLine("Incorrect input. Try again.");
                        break;
                }
                context.SaveChanges();
                Console.WriteLine("Computer updated");
            }
        }

        /*
         * This method is showinng all phones.
         */
        private void ShowAllPhones()
        {

            foreach (var x in context.Phones)
            {
                Console.WriteLine($"{x.Id}. Brand: {x.BrandName}, Model: {x.ModelName}, Price: {CurrencyConversion(x.Office, x.Price)},  Purchase date: {x.PurchaseDate}, Office: {x.Office}");
            }

        }

        /*This method is showing all computers.
         */
        private void ShowAllComputers()
        {

            foreach (var x in context.Computers)
            {
                Console.WriteLine($"{x.Id}. Brand: {x.BrandName}, Model: {x.ModelName}, Price: {CurrencyConversion(x.Office, x.Price)},  Purchase date: {x.PurchaseDate}, Office: {x.Office}");
            }

        }


        /* Method is sorting items when first it shows all computers and then all phones. 
         * 
         * 
         */
        public void SortedListComputerFirst()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Clear();
            //sort computers
            
            if (context.Computers.Any())
            {
                var computers = context.Computers.OrderBy(c => c.BrandName).ToList();
                Console.WriteLine("Computers:\n");
                foreach (var c in computers)
                {
                    Console.WriteLine($"{c.Id}. Brand: {c.BrandName}, Model: {c.ModelName}, Price: {CurrencyConversion(c.Office, c.Price)},  Purchase date: {c.PurchaseDate}, Office: {c.Office}");

                }
            }
            else
            {
                Console.WriteLine("Computers:\n");
                Console.WriteLine("No computers added.\n");
            }

            //sort phones
            if(context.Phones.Any())
            {
                var phones = context.Phones.OrderBy(p => p.BrandName).ToList();

                Console.WriteLine("\nPhones:\n");
                foreach (var p in phones)
                {
                    Console.WriteLine($"{p.Id}. Brand: {p.BrandName}, Model: {p.ModelName}, Price: {CurrencyConversion(p.Office, p.Price)},  Purchase date: {p.PurchaseDate}, Office: {p.Office}");

                }
            }
            else
            {
                Console.WriteLine("\nPhones:\n");
                Console.WriteLine("No phones added.\n");
            }
        }


        /**
         * Method is adding all itmes in one list and then converting their purchase date to integer. 
         * Then it substract purchase date from todays date, for example 20201130 (is 30 November 2020)
         * minus 20180505 (5 May 2018) is 20625 which means 2 years and 6 months and 25 days so the item will be marked as Yellow.
         * Maybe there is more appropriate method to do that but I couldn't find it. 
         */
        public void SortListByPurchaseDate()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            List<Electronics> electro = new List<Electronics>();
            foreach (var item in context.Computers)
            {
                electro.Add(item);
            }
            foreach (var item in context.Phones)
            {
                electro.Add(item);
            }


            int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));

            electro = electro.OrderBy(c => Convert.ToDateTime(c.PurchaseDate))
                .ToList();

            Console.WriteLine("\nSorted list by purchase date:\n");
            foreach (var item in electro)
            {
                var fixdate = item.PurchaseDate.Replace(".", "");
                int date = int.Parse(fixdate);
                int RedDate = now - date;
                if (RedDate > 20900)
                {
                    Console.WriteLine($"{item.Id}. Brand: {item.BrandName}, Model: {item.ModelName}, Price: {CurrencyConversion(item.Office, item.Price)},  Purchase date: {item.PurchaseDate}, Office: {item.Office} *RED*");

                }
                else if (RedDate > 20600)
                {
                    Console.WriteLine($"{item.Id}. Brand: {item.BrandName}, Model: {item.ModelName}, Price: {CurrencyConversion(item.Office, item.Price)},  Purchase date: {item.PurchaseDate}, Office: {item.Office} *YELLOW*");

                }
                else
                {
                    Console.WriteLine($"{item.Id}. Brand: {item.BrandName}, Model: {item.ModelName}, Price: {CurrencyConversion(item.Office, item.Price)},  Purchase date: {item.PurchaseDate}, Office: {item.Office}");

                }
            }

        }



        /*
         * This method is sorting list by office
         * It puts all itmes in one list.
         * Then it sorting list by itmes office.
         * 
         */
        public void SortedListByOffice()
        {
            Console.ForegroundColor = ConsoleColor.White;
            List<Electronics> electro = new List<Electronics>();
            foreach (var item in context.Computers)
            {
                electro.Add(item);
            }
            foreach (var item in context.Phones)
            {
                electro.Add(item);
            }

            electro = electro.OrderBy(p => p.Office).ToList();


            Console.WriteLine("\nSorted list by office:\n");
            foreach (var e in electro)
            {

                Console.WriteLine($"{e.Id}. Brand: {e.BrandName}, Model: {e.ModelName}, Price: {CurrencyConversion(e.Office, e.Price)},  Purchase date: {e.PurchaseDate}, Office: {e.Office}");

            }
        }


        /*
         * This method is converting price in dollars to currency of the office country.
         * It is using api to do that.
         * It is also using NewtonSoft Json extension because api data is written in json. 
         * 
         */

        public string CurrencyConversion(string office, string price)
        {
            string fromCurrency1 = "usd";
            string toCurrency1;
            switch (office)
            {
                case "Sweden":
                    toCurrency1 = "sek";
                    break;
                case "Poland":
                    toCurrency1 = "pln";
                    break;
                default:
                    toCurrency1 = "cad";
                    break;
            }

            decimal amount1 = Convert.ToDecimal(price);

            const string urlPattern = "http://rate-exchange-1.appspot.com/currency?from={0}&to={1}";
            string url = string.Format(urlPattern, fromCurrency1, toCurrency1);

            using (var wc = new WebClient())
            {
                var json = wc.DownloadString(url);

                Newtonsoft.Json.Linq.JToken token = Newtonsoft.Json.Linq.JObject.Parse(json);
                decimal exchangeRate = (decimal)token.SelectToken("rate");

                return $"{amount1 * exchangeRate:N3} {toCurrency1.ToUpper()}";

            }
        }



    }
}

