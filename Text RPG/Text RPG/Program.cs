﻿using System;
using System.Collections.Generic;
class Program
{
    static void Main()
    {
        Character player = new Character
        {
            Level = 1,
            Name = "name",
            Job = "전사",
            Attack = 10,
            Defense = 5,
            HP = 100,
            Gold = 1500
        };

        Shop shop = new Shop(); // 상점 생성

        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== 스파르타 던전에 오신 것을 환영합니다! =====");
            Console.WriteLine("1. 캐릭터 상태 보기");
            Console.WriteLine("2. 인벤토리 확인");
            Console.WriteLine("3. 상점 방문");
            Console.WriteLine("0. 게임 종료");
            Console.Write("\n당신의 선택: ");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    player.ShowInfo();
                    break;
                case "2":
                    player.inventory.ShowInventory();
                    break;
                case "3":
                    shop.ShowShop(player);
                    break;
                case "0":
                    Console.WriteLine("게임을 종료합니다.");
                    return;
                default:
                    Console.WriteLine("잘못된 입력입니다. 0~3 사이의 숫자를 입력하세요.");
                    break;
            }

            Console.WriteLine("\n아무 키나 누르면 계속...");
            Console.ReadKey();
        }
    }
}
class Item
{
    public string Name { get; }
    public string Type { get; } // "칼", "창", "방어구"
    public int StatBoost { get; } // 공격력 or 방어력 증가량
    public int Price { get; } // 가격
    public string Description { get; } // 설명
    public bool IsEquipped { get; set; } // 장착 여부

    public Item(string name, string type, int statBoost, int price, string description)
    {
        Name = name;
        Type = type;
        StatBoost = statBoost;
        Price = price;
        Description = description;
        IsEquipped = false;
    }
}

class Inventory
{
    private List<Item> items = new List<Item>();

    public void AddItem(Item item)
    {
        items.Add(item);
    }

    public bool HasItem(string itemName)
    {
        return items.Exists(item => item.Name == itemName);
    }

    public void ShowInventory()
    {
        Console.Clear();
        Console.WriteLine("===== [인벤토리] =====");

        if (items.Count == 0)
        {
            Console.WriteLine("보유 중인 아이템이 없습니다.");
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                string equippedMark = items[i].IsEquipped ? "[E] " : "";
                Console.WriteLine($"{i + 1}. {equippedMark}{items[i].Name} ({items[i].Type}, +{items[i].StatBoost})");
            }
        }

        Console.WriteLine("\n1. 장착 관리");
        Console.WriteLine("0. 나가기");
        Console.Write("\n당신의 선택: ");

        while (true)
        {
            string input = Console.ReadLine();
            if (input == "0")
                return;
            else if (input == "1")
                ShowEquipmentMenu();
            else
                Console.WriteLine("잘못된 입력입니다. 0 또는 1을 입력하세요.");
        }
    }

    public void ShowEquipmentMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== [장착 관리] =====");

            if (items.Count == 0)
            {
                Console.WriteLine("보유 중인 아이템이 없습니다.");
                return;
            }

            for (int i = 0; i < items.Count; i++)
            {
                string equippedMark = items[i].IsEquipped ? "[E] " : "";
                Console.WriteLine($"{i + 1}. {equippedMark}{items[i].Name} ({items[i].Type}, +{items[i].StatBoost})");
            }

            Console.WriteLine("0. 나가기");
            Console.Write("\n장착할 아이템 번호를 선택하세요: ");
            string input = Console.ReadLine();

            if (input == "0")
            {
                Console.WriteLine("장착 관리에서 나갑니다.");
                return;
            }

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= items.Count)
            {
                EquipItem(choice - 1);
            }
            else
            {
                Console.WriteLine("올바른 번호를 입력하세요.");
            }
        }
    }

    public void EquipItem(int index)
    {
        Item selectedItem = items[index];

        if (selectedItem.IsEquipped)
        {
            selectedItem.IsEquipped = false;
            Console.WriteLine($"{selectedItem.Name}을(를) 장착 해제했습니다.");
        }
        else
        {
            foreach (var item in items)
            {
                if (item.Type == selectedItem.Type && item.IsEquipped)
                {
                    item.IsEquipped = false;
                    Console.WriteLine($"{item.Name}을(를) 해제했습니다.");
                }
            }

            selectedItem.IsEquipped = true;
            Console.WriteLine($"{selectedItem.Name}을(를) 장착했습니다!");
        }
    }
}

class Character
{
    public int Level { get; set; }
    public string Name { get; set; }
    public string Job { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int HP { get; set; }
    public int Gold { get; set; }
    public Inventory inventory { get; set; } = new Inventory();

    public void ShowInfo()
    {
        Console.Clear();
        Console.WriteLine("===== [캐릭터 상태] =====");
        Console.WriteLine($"Lv. {Level}");
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"Chad: ( {Job} )");
        Console.WriteLine($"공격력: {Attack}");
        Console.WriteLine($"방어력: {Defense}");
        Console.WriteLine($"체력: {HP}");
        Console.WriteLine($"Gold: {Gold}G");
        Console.WriteLine("=========================");
        Console.WriteLine("0. 나가기");

        while (true)
        {
            string input = Console.ReadLine();
            if (input == "0") return;
            else Console.WriteLine("잘못된 입력입니다. 0을 입력하세요.");
        }
    }
}

class Shop
{
    private List<Item> shopItems = new List<Item>
    {
        new Item("강철 검", "칼", 15, 1000, "강한 공격력을 가진 검"),
        new Item("철 방패", "방어구", 10, 800, "튼튼한 방어력을 제공하는 방패"),
        new Item("마법 반지", "반지", 5, 500, "마법의 힘을 지닌 반지")
    };

    public void ShowShop(Character player)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== [상점] =====");
            Console.WriteLine($"[보유 골드]: {player.Gold}G");
            Console.WriteLine("\n[아이템 목록]");

            for (int i = 0; i < shopItems.Count; i++)
            {
                Item item = shopItems[i];
                string status = player.inventory.HasItem(item.Name) ? "구매완료" : $"{item.Price}G";
                Console.WriteLine($"{i + 1}. {item.Name} ({item.Type}, +{item.StatBoost}) | {status} | {item.Description}");
            }

            Console.WriteLine("\n1. 아이템 구매");
            Console.WriteLine("0. 나가기");
            Console.Write("\n당신의 선택: ");

            string input = Console.ReadLine();
            if (input == "0") return;
            else if (input == "1") BuyItem(player);
            else Console.WriteLine("올바른 입력이 아닙니다.");
        }
    }

    private void BuyItem(Character player)
    {
        Console.Write("\n구매할 아이템 번호를 입력하세요 (0: 취소): ");
        string input = Console.ReadLine();

        if (input == "0") return;

        if (int.TryParse(input, out int choice) && choice >= 1 && choice <= shopItems.Count)
        {
            Item selectedItem = shopItems[choice - 1];

            if (player.inventory.HasItem(selectedItem.Name))
            {
                Console.WriteLine("이미 구매한 아이템입니다!");
                return;
            }

            if (player.Gold >= selectedItem.Price)
            {
                player.Gold -= selectedItem.Price;
                player.inventory.AddItem(selectedItem);
                Console.WriteLine($"{selectedItem.Name}을(를) 구매했습니다!");
            }
            else Console.WriteLine("골드가 부족합니다!");
        }
        else Console.WriteLine("올바른 번호를 입력하세요.");
    }
}
