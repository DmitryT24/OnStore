using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnStore
{
    // Общий для различных способов доставки
    abstract class IDelivery : StatusDelivery // 
    {
        public string Adress { get; set; }

        public abstract void Delivery(string adressCity);
        public string[] AdressCity = { "Москва", "Смоленск", "Омск", "Воронеж" };
        public bool isDelivery;
    }
    //
    class StatusDelivery
    {
        public string Status;
        public string DateDeli { get; set; } // дата доставки
        //нужен метод варианта перевозки товара
        public void DisplayInfo()
        {
            Console.WriteLine($" Статус заказа: {Status} \nДоставка: {DateDeli}");
        }

    }
    class HomeDelivery : IDelivery
    {
        //  public string Address { get; set; }
        public bool isCourier { get; set; }
        public string SpecialInstructions { get; set; }
        public override void Delivery(string adressCity)
        {
            if (AdressCity.Contains(adressCity))
            {
                isDelivery = true;
                Console.WriteLine("В ваш город возможна доставка!");
            }
            else
            {
                isDelivery = false;
                Console.WriteLine("Извените, в ваш город нет доставки!");
            }
        }
    }

    class PickPointDelivery : IDelivery
    {

        public string PickPointDetails { get; set; } // купить пакет 
        public override void Delivery(string adressCity)
        {
            if (AdressCity.Contains(adressCity))
            {
                isDelivery = true;
                Console.WriteLine("В ваш город возможна доставка!");
            }
            else
            {
                isDelivery = false;
                Console.WriteLine("Извените, в ваш город нет доставки!");
            }
        }
    }

    class ShopDelivery : IDelivery
    {

        public string PShopDeliveryDetails { get; set; } // купить пакет 
        public string ShopLocation { get; set; }
        public override void Delivery(string adressCity)
        {
            if (AdressCity.Contains(adressCity))
            {
                isDelivery = true;
                Console.WriteLine("В ваш город возможна доставка!");
            }
            else
            {
                isDelivery = false;
                Console.WriteLine("Извените, в ваш город нет доставки!");
            }
        }
    }

    class OnlineShop : Product
    {
        Product[] products;// = new Product[] { };
        public OnlineShop(Product[] products) { this.products = products; }
        public void Display()
        {
            foreach (var p in products)
            {
                Console.WriteLine($"Id:{p.Id}\t - Product: {p.Name}\t - Price: {p.Price}\t - Quantity: {p.Quantity}\t");
            }
        }
    }
    //Склад размещения
    class Storage
    {
        uint Count; //кол-во товара в корзинах покупателей
        public uint Quantity { get; set; } // количество  данного товара на складе
        public string NameStore { get; set; } // склад расположения 

        bool isSize { get; set; } // габаритный товар


    }

    // Класс продукта
    class Product : Storage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public bool isDiscount { get; set; }

    }

    // Класс корзины
    class ShoppingBasket : Product
    {
        public uint CountProd = 0;
        public List<Product> products = new List<Product>(); /// private

        public void AddProduct(Product product)
        {
            products.Add(product); CountProd++;
        }

        public void RemoveProduct(int productId)
        {
            products.RemoveAll(p => p.Id == productId); CountProd--;
        }

        public void DisplayBasket()
        {
            foreach (var product in products)
            {
                Console.WriteLine($"Id: {product.Id} \n Product: {product.Name}\t - Price: {product.Price}\t - Quantity: {product.Quantity}\t");
            }
            Console.WriteLine();
        }


    }


    // Методы расширения для класса ShoppingBasket
    static class ShoppingBasketExtensions
    {
        public static decimal CalculateTotal(this ShoppingBasket basket)
        {
            decimal total = 0;
            decimal totalNoDisc = 0;
            foreach (var product in basket.products)
            {

                if (product.isDiscount)
                    totalNoDisc += product.Price * (1.0M - (product.Discount / 100.0M));
                else
                    total += product.Price;
            }
            return total;
        }
    }

    class UserOrder
    {
        bool isStop = true;
        Product[] products;
        ShoppingBasket shopBasketUs;
        Product[] ShopProductInit()
        {
            Product[] productsShop = new Product[] {
               new Product { Id = 1, Name = "Фильтр",       Price = 400, isDiscount = true,  Discount = 17, Quantity = 100, NameStore = "Москва" },
               new Product { Id = 2, Name = "Колесо",      Price = 800, isDiscount = false, Discount = 0,  Quantity = 100, NameStore = "Москва" },
               new Product { Id = 3, Name = "Щетка",       Price = 800, isDiscount = false, Discount = 0,  Quantity = 100, NameStore = "Смоленск" },
               new Product { Id = 4, Name = "Лампочка",    Price = 50,  isDiscount = true,  Discount = 10, Quantity = 80,  NameStore = "Коломна" },
               new Product { Id = 5, Name = "Вилка",    Price = 50,  isDiscount = false, Discount = 0,  Quantity = 80,  NameStore = "Королев" },
               new Product { Id = 6, Name = "Провод",      Price = 50,  isDiscount = false, Discount = 0,  Quantity = 80,  NameStore = "Королев" },
               new Product { Id = 7, Name = "Аккумулятор", Price = 40,  isDiscount = false, Discount = 0,  Quantity = 80,  NameStore = "Новгород" },
               new Product { Id = 8, Name = "Диск", Price = 50,  isDiscount = true,  Discount = 5,  Quantity = 80,  NameStore = "Псков" }
            };
            return productsShop;

        }
        public UserOrder()
        {
            products = ShopProductInit();
        }

        void SetBasketUs(out ShoppingBasket shopBasket)
        {
            OnlineShop onShop = new OnlineShop(products);

            /*ShoppingBasket*/
            shopBasket = new ShoppingBasket();
            do
            {
                bool isFlag = false;
                Console.WriteLine(" Для перехода в корзину введите: stop");

                onShop.Display();
                Console.WriteLine();
                Console.WriteLine(" Добавьте необходимый товар в корзину указав его имя списка выше.");
                // Console.SetCursorPosition(0, products.Length + 2);

                Console.WriteLine($"Кол-во товаров в корзине: {shopBasket.CountProd}");
                Console.Write(new String(' ', Console.BufferWidth));
                Console.Write("Название товара: ");
                string stroka = Console.ReadLine();

                if (stroka == "stop")
                {
                    isStop = false;
                    break;
                }

                foreach (Product pr in products)
                {
                    if (pr.Name == stroka)
                    {
                        shopBasket.AddProduct(pr);
                        Console.Write("Товар добавлен в корзиную");
                        Console.Clear();
                        isFlag = true;
                    }
                }
                // Console.SetCursorPosition(0, products.Length + 2);
                if (!isFlag)
                {
                    Console.Clear();
                    Console.WriteLine("Данный товар отсутствует, повторите ввод.");
                }


            } while (isStop);
        }

        void СhoiceBasket()
        {
            isStop = true;
            do
            {
                int id;
                Console.Clear();
                Console.WriteLine("Для удаления товара из корзины введите номер Id.");
                Console.WriteLine("Для оформления введите - next.");

                shopBasketUs.DisplayBasket();
                string stroka = Console.ReadLine();

                if (stroka == "next") { isStop = false; break; }
                if (int.TryParse(stroka, out id))
                    shopBasketUs.RemoveProduct(id);

            } while (isStop);

            Console.WriteLine($" Заказ на сумму: {shopBasketUs.CalculateTotal()}");
        }

        void DecorUser()
        {
            //Вариант доставки товаров
            Console.Clear();
            Console.WriteLine("Москва Смоленск Омск Воронеж");
            Console.Write("Укажите ваш город:");

            string adressUser = Console.ReadLine();

            Console.WriteLine("Укажите вариант доставки:" +
                "\n 1 - Пункт выдачи;" +
                "\n 2 - Магазин" +
                "\n 3 - Курьер");

            var TipDelivery = int.Parse(Console.ReadLine());
            StatusDelivery statusDelivery = null;
            switch (TipDelivery)
            {
                case 1:
                    PickPointDelivery pickPointDelivery = new PickPointDelivery { PickPointDetails = "+ Пакет", Status = " Оформление", DateDeli = "срок доставки 2 дня" };
                    pickPointDelivery.Delivery(adressUser);
                    statusDelivery = pickPointDelivery;

                    break;
                case 2:
                    ShopDelivery shopDelivery = new ShopDelivery { PShopDeliveryDetails = "+ Пакет", Status = " Оформление", DateDeli = "срок доставки 1 дня" };
                    shopDelivery.Delivery(adressUser);
                    statusDelivery = shopDelivery;
                    break;
                case 3:
                    HomeDelivery homeDelivery = new HomeDelivery { isCourier = true, SpecialInstructions = "Дорставка на этаж", Status = " Доставка", DateDeli = "срок доставки в течение часа" };
                    homeDelivery.Delivery(adressUser);
                    statusDelivery = homeDelivery;
                    break;
                default:
                    Console.WriteLine("УПС! Нет такой доставки!");
                    break;
            }

            statusDelivery.DisplayInfo();
        }

        public void ShoppingUser()
        {
            SetBasketUs(out shopBasketUs);
            СhoiceBasket();
            DecorUser();
        }
    }


    class Program
    {

        static void Main()
        {
            UserOrder userOrder = new UserOrder();
            userOrder.ShoppingUser();
        }
    }
}







