using System;
using System.Collections.Generic;
using System.IO;

// 아이템 타입 열거형 정의
enum ItemType
{
    Armor,
    Weapon
}

// 아이템 정보를 클래스로 정의
class Item
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int AttackBonus { get; set; }
    public int DefenseBonus { get; set; }
    public int Price { get; set; }
    public bool Equipped { get; set; }
    public ItemType Type { get; set; }

    public Item(string name, string description, int attackBonus, int defenseBonus, int price, ItemType type)
    {
        Name = name;
        Description = description;
        AttackBonus = attackBonus;
        DefenseBonus = defenseBonus;
        Price = price;
        Equipped = false;
        Type = type;
    }

    public override string ToString()
    {
        return $"{Name} | {Description}";
    }
}

// 캐릭터 클래스 정의
class Character
{
    public string Name { get; set; }
    public string CharClass { get; set; }
    public int Level { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Health { get; set; }
    public int Gold { get; set; }
    public List<Item> Inventory { get; set; }

    // 장착된 아이템 변수 정의
    private Item equippedArmor;
    private Item equippedWeapon;

    public Character(string name, string charClass, int attack, int defense, int health, int gold)
    {
        Name = name;
        CharClass = charClass;
        Attack = attack;
        Defense = defense;
        Health = health;
        Gold = gold;
        equippedArmor = null;
        equippedWeapon = null;
        Inventory = new List<Item>();
    }

    // 인벤토리 관리 메서드
    public void ManageInventory()
    {
        Console.WriteLine("**인벤토리**");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");

        // 아이템 목록 출력
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (Inventory[i].Equipped)
                Console.WriteLine($"- {i + 1} [E] {Inventory[i]}");
            else
                Console.WriteLine($"- {i + 1} {Inventory[i]}");
        }

        Console.WriteLine("0. 나가기\n");
        Console.WriteLine("원하시는 행동을 입력해주세요.");
    }

    // 아이템 장착 메서드
    public void EquipItemToCharacter(Item item)
    {
        if (item == null)
        {
            Console.WriteLine("잘못된 아이템입니다.");
            return;
        }

        switch (item.Type)
        {
            case ItemType.Armor:
                if (equippedArmor != null)
                {
                    UnequipItem(equippedArmor);
                }
                equippedArmor = item;
                Console.WriteLine($"{item.Name}을(를) 장착하였습니다.");
                break;
            case ItemType.Weapon:
                if (equippedWeapon != null)
                {
                    UnequipItem(equippedWeapon);
                }
                equippedWeapon = item;
                Console.WriteLine($"{item.Name}을(를) 장착하였습니다.");
                break;
            default:
                Console.WriteLine("잘못된 아이템입니다.");
                break;
        }
    }

    // 아이템 해제 메서드
    public void UnequipItem(Item item)
    {
        if (item == null)
        {
            Console.WriteLine("해제할 아이템이 없습니다.");
            return;
        }

        switch (item.Type)
        {
            case ItemType.Armor:
                if (equippedArmor == item)
                {
                    equippedArmor = null;
                    Console.WriteLine($"{item.Name}을(를) 해제하였습니다.");
                }
                else
                {
                    Console.WriteLine($"{item.Name}이(가) 장착되어 있지 않습니다.");
                }
                break;
            case ItemType.Weapon:
                if (equippedWeapon == item)
                {
                    equippedWeapon = null;
                    Console.WriteLine($"{item.Name}을(를) 해제하였습니다.");
                }
                else
                {
                    Console.WriteLine($"{item.Name}이(가) 장착되어 있지 않습니다.");
                }
                break;
            default:
                Console.WriteLine("해제할 아이템이 없습니다.");
                break;
        }
    }

    // 저장 기능 추가
    public void Save(string fileName)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            writer.WriteLine($"{Name},{CharClass},{Level},{Attack},{Defense},{Health},{Gold}");

            foreach (var item in Inventory)
            {
                writer.WriteLine($"{item.Name},{item.Description},{item.AttackBonus},{item.DefenseBonus},{item.Price},{item.Type},{item.Equipped}");
            }
        }
    }

    // 파일로부터 캐릭터 정보를 로드하는 메서드
    public static Character Load(string filename)
    {
        Character character = new Character("", "", 0, 0, 0, 0);

        using (StreamReader reader = new StreamReader(filename))
        {
            character.Name = reader.ReadLine();
            character.CharClass = reader.ReadLine();
            character.Level = int.Parse(reader.ReadLine());
            character.Attack = int.Parse(reader.ReadLine());
            character.Defense = int.Parse(reader.ReadLine());
            character.Health = int.Parse(reader.ReadLine());
            character.Gold = int.Parse(reader.ReadLine());

            // 인벤토리 아이템 로드
            int itemCount = int.Parse(reader.ReadLine());
            for (int i = 0; i < itemCount; i++)
            {
                string name = reader.ReadLine();
                string description = reader.ReadLine();
                int attackBonus = int.Parse(reader.ReadLine());
                int defenseBonus = int.Parse(reader.ReadLine());
                int price = int.Parse(reader.ReadLine());
                bool equipped = bool.Parse(reader.ReadLine());
                ItemType type = (ItemType)Enum.Parse(typeof(ItemType), reader.ReadLine());

                Item item = new Item(name, description, attackBonus, defenseBonus, price, type);
                item.Equipped = equipped;
                character.Inventory.Add(item);
            }
        }

        return character;
    }
}

class MainClass
{
    static void Main(string[] args)
    {
        string saveFileName = "save.txt";

        // 아이템 정보 배열
        Item[] itemList = {
        new Item("해신작쇼", "방어력 +5 | 수련에 도움을 주는 갑옷입니다.", 0, 5, 1000, ItemType.Armor),
        new Item("가시갑옷", "방어력 +9 | 무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 9, 1800, ItemType.Armor),
        new Item("건틀릿", "방어력 +15 | 욤마의 전사들이 사용했다는 전설의 갑옷입니다.", 0, 15, 3500, ItemType.Armor),
        new Item("단검", "공격력 +2 | 쉽게 볼 수 있는 낡은 검 입니다.", 2, 0, 600, ItemType.Weapon),
        new Item("대검", "공격력 +5 | 어디선가 사용됐던거 같은 검입니다.", 5, 0, 1500, ItemType.Weapon),
        new Item("그림자검", "공격력 +7 | 욤마의 전사들이 사용했다는 전설의 창입니다.", 7, 0, 2500, ItemType.Weapon)
    };

        // 게임 시작 시, 저장된 파일이 있다면 로드하고 없다면 새로운 캐릭터 생성
        Character player;
        if (File.Exists(saveFileName))
        {
            player = Character.Load(saveFileName);
            Console.WriteLine("이전 저장된 게임을 로드합니다.");
        }
        else
        {
            player = new Character("Chad", "전사", 10, 5, 100, 1500);
            Console.WriteLine("새로운 게임을 시작합니다.");
        }

        // 처음 환영 메시지 출력
        Console.WriteLine("욤마 마을에 오신 여러분 환영합니다.");
        Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");

        while (true)
        {
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전입장");
            Console.WriteLine("5. 휴식하기");
            Console.WriteLine("6. 저장하기");
            Console.WriteLine("0. 게임 종료\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ShowStatus(player);
                    break;
                case "2":
                    // 인벤토리 관리
                    player.ManageInventory();
                    break;
                case "3":
                    // 상점
                    ShopMenu(player, itemList);
                    break;
                case "4":
                    // 던전 입장
                    DungeonEntrance(player);
                    break;
                case "5":
                    // 휴식하기
                    Rest(player);
                    break;
                case "6":
                    // 저장하기
                    player.Save(saveFileName);
                    Console.WriteLine("게임이 저장되었습니다.");
                    break;
                case "0":
                    // 게임 종료
                    Console.WriteLine("게임을 종료합니다.");
                    return;
                default:
                    Console.WriteLine("잘못된 입력입니다.\n");
                    break;
            }
            Console.WriteLine(); // 메뉴 구분을 위한 줄바꿈
        }
    }


    static void ShowStatus(Character player)
    {
        Console.WriteLine("**상태 보기**");
        Console.WriteLine("캐릭터의 정보를 표시합니다.\n");
        Console.WriteLine($"Lv. {player.Level}");
        Console.WriteLine($"{player.Name} ( {player.CharClass} )");
        Console.WriteLine($"공격력 : {player.Attack}");
        Console.WriteLine($"방어력 : {player.Defense}");
        Console.WriteLine($"체 력 : {player.Health}");
        Console.WriteLine($"Gold : {player.Gold} G\n");
    }

    static void ShopMenu(Character player, Item[] itemList)
    {
        while (true)
        {
            Console.WriteLine("**상점**");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
            Console.WriteLine($"[보유 골드] {player.Gold} G\n");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < itemList.Length; i++)
            {
                Console.WriteLine($"- {itemList[i].Name} | {itemList[i].Description} | {itemList[i].Price} G");
            }
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    BuyItem(player, itemList);
                    break;
                case "2":
                    SellItem(player, itemList);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("잘못된 입력입니다.\n");
                    break;
            }
            Console.WriteLine(); // 메뉴 구분을 위한 줄바꿈
        }
    }

    static void BuyItem(Character player, Item[] itemList)
    {
        Console.WriteLine("**아이템 구매**");
        Console.WriteLine("구매할 아이템의 번호를 입력하세요 (0: 나가기):");
        for (int i = 0; i < itemList.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {itemList[i].Name} | {itemList[i].Description} | {itemList[i].Price} G");
        }

        if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex >= 1 && itemIndex <= itemList.Length)
        {
            Item selected = itemList[itemIndex - 1];
            if (player.Gold >= selected.Price)
            {
                player.Inventory.Add(selected);
                player.Gold -= selected.Price;
                Console.WriteLine($"{selected.Name}을(를) 구매했습니다.");
            }
            else
            {
                Console.WriteLine("Gold가 부족합니다.");
            }
        }
        else if (itemIndex == 0)
        {
            Console.WriteLine("상점으로 돌아갑니다.");
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
        }
    }

    static void SellItem(Character player, Item[] itemList)
    {
        Console.WriteLine("**아이템 판매**");
        Console.WriteLine("판매할 아이템의 번호를 입력하세요 (0: 나가기):");
        player.ManageInventory();

        if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex >= 1 && itemIndex <= player.Inventory.Count)
        {
            Item selected = player.Inventory[itemIndex - 1];
            player.Inventory.RemoveAt(itemIndex - 1);
            int sellPrice = (int)(selected.Price * 0.85); // 판매 가격은 구매 가격의 85%
            player.Gold += sellPrice;
            Console.WriteLine($"{selected.Name}을(를) 판매했습니다. (+{sellPrice} G)");
        }
        else if (itemIndex == 0)
        {
            Console.WriteLine("상점으로 돌아갑니다.");
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
        }
    }

    static void Rest(Character player)
    {
        Console.WriteLine("**휴식하기**");
        Console.WriteLine($"500 G를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.Gold} G)\n");
        Console.WriteLine("1. 휴식하기");
        Console.WriteLine("0. 나가기\n");

        if (int.TryParse(Console.ReadLine(), out int choice) && (choice == 1 || choice == 0))
        {
            switch (choice)
            {
                case 1:
                    if (player.Gold >= 500)
                    {
                        player.Health = Math.Min(player.Health + 100, 100); // 최대 체력은 100
                        player.Gold -= 500;
                        Console.WriteLine("휴식을 완료했습니다.");
                    }
                    else
                    {
                        Console.WriteLine("Gold가 부족합니다.");
                    }
                    break;
                case 0:
                    Console.WriteLine("상점으로 돌아갑니다.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
        }
    }

    static void DungeonEntrance(Character player)
    {
        Console.WriteLine("**던전 입장**");
        Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
        Console.WriteLine("1. 쉬운 던전     | 방어력 5 이상 권장");
        Console.WriteLine("2. 일반 던전     | 방어력 11 이상 권장");
        Console.WriteLine("3. 어려운 던전    | 방어력 17 이상 권장");

        Console.WriteLine("원하시는 던전을 선택하세요:");
        if (int.TryParse(Console.ReadLine(), out int dungeonChoice))
        {
            switch (dungeonChoice)
            {
                case 1:
                    EnterDungeon(player, "쉬운 던전", 5, 1000);
                    break;
                case 2:
                    EnterDungeon(player, "일반 던전", 11, 1700);
                    break;
                case 3:
                    EnterDungeon(player, "어려운 던전", 17, 2500);
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
        }
    }

    static void EnterDungeon(Character player, string dungeonName, int recommendedDefense, int baseReward)
    {
        Console.WriteLine($"**{dungeonName} 입장**");
        Console.WriteLine($"권장 방어력: {recommendedDefense}");
        Console.WriteLine($"현재 방어력: {player.Defense}");

        if (player.Defense < recommendedDefense)
        {
            Console.WriteLine($"방어력이 부족하여 {dungeonName}을 클리어할 수 없습니다.");
            int halfHealthLoss = player.Health / 2;
            player.Health -= halfHealthLoss;
            Console.WriteLine($"체력이 {halfHealthLoss} 감소합니다.");
        }
        else
        {
            Console.WriteLine($"방어력이 충분하여 {dungeonName}을 클리어할 수 있습니다.");
            Random rand = new Random();
            int healthLoss = rand.Next(20, 36) - (player.Defense - recommendedDefense);
            player.Health -= healthLoss;
            Console.WriteLine($"체력이 {healthLoss} 감소합니다.");

            int additionalRewardPercentage = rand.Next(player.Attack, player.Attack * 2 + 1);
            int additionalReward = (int)(baseReward * (additionalRewardPercentage / 100.0));
            int totalReward = baseReward + additionalReward;

            player.Gold += totalReward;
            Console.WriteLine($"던전 클리어! 보상으로 {totalReward} G를 획득했습니다.");
        }
    }
}