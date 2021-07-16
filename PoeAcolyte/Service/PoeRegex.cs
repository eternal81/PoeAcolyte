using System.Text.RegularExpressions;

namespace PoeAcolyte.Service
{
    /// <summary>
    /// Regex arrays bounced against client.txt file for matches in multiple languages
    /// </summary>
    public static class PoeRegex
    {
        /// <summary>
        /// REGEX to determine if @From is found (meaning a whisper from someone) <br></br>
        /// (?:From|De|От кого|จาก|Von|Desde|수신|來自) (?&lt;Player&gt;.*?): (?&lt;Other&gt;.*)
        /// <value>Player, Other</value>
        /// </summary>
        public static readonly Regex WhisperFrom = new Regex(@"@(?:From|De|От кого|จาก|Von|Desde|수신|來自) (?<Player>.*?): (?<Other>.*)");
        
        /// <summary>
        /// REGEX to determine if @To is found (meaning a whisper to someone) <br></br>
        /// (?:To|À|An|Para|Кому|ถึง|발신|向) (?&lt;Player&gt;.*?): (?&lt;Other&gt;.*)
        /// <value>Player, Other</value> 
        /// </summary>
        public static readonly Regex WhisperTo = new Regex(@"@(?:To|À|An|Para|Кому|ถึง|발신|向) (?<Player>.*?): (?<Other>.*)");
        
        /// <summary>
        /// REGEX for guild and player name <br></br>
        /// &lt;(?&lt;Guild&gt;.*?)&gt; (?&lt;Player&gt;.*?): 
        /// <value>Guild, Player</value>
        /// </summary>
        public static readonly Regex Guild = new Regex(@"<(?<Guild>.*?)> (?<Player>.*?): ");

        /// <summary>
        /// Regex used as a catch all <br></br>
        /// (.*?)] (?&lt;Message&gt;.*)
        /// <value>Message</value>
        /// </summary>
        public static readonly Regex SystemMessage = new Regex(@"(.*?)] (?<Message>.*)");
        
        /// <summary>
        /// REGEX for Regular Priced trades - will need to parse <see cref="StashTabList"/><br></br>
        /// (.*)Hi, I would like to buy your (?&lt;Item&gt;.*) listed for (?&lt;PriceAmount&gt;\d+) (?&lt;PriceUnit&gt;.*) in (?&lt;League&gt;.*?)\(
        /// <value>Item, PriceAmount, PriceUnit, League</value>
        /// </summary>
        public static readonly Regex[] PricedTradeList =
        {
            /* ENG */ new Regex(@"(.*)Hi, I would like to buy your (?<Item>.*) listed for (?<PriceAmount>\d+) (?<PriceUnit>.*) in (?<League>.*?)\(", RegexOptions.Compiled),
            /* RUS */ new Regex(@"(.*)Здравствуйте, хочу купить у вас (?<Item>.*) за (?<PriceAmount>\d+) (?<PriceUnit>.*) в лиге (?<League>.*?)\(", RegexOptions.Compiled), 
            /* POR */ new Regex(@"(.*)Olá, eu gostaria de comprar o seu item (?<Item>.*) listado por (?<PriceAmount>\d+) (?<PriceUnit>.*) na (?<League>.*?)\(", RegexOptions.Compiled),
            /* THA */ new Regex(@"(.*)สวัสดี, เราต้องการจะชื้อของคุณ (?<Item>.*) ใน ราคา (?<PriceAmount>\d+) (?<PriceUnit>.*) ใน (?<League>.*?)\(", RegexOptions.Compiled),
            /* GER */ new Regex(@"(.*)Hi, ich möchte '(?<Item>.*)' zum angebotenen Preis von (?<PriceAmount>\d+) (?<PriceUnit>.*) in der '(?<League>.*?)'-Liga kaufen(.*)", RegexOptions.Compiled),
            /* FRE */ new Regex(@"(.*)Bonjour, je souhaiterais t'acheter (?<Item>.*) pour (?<PriceAmount>\d+) (?<PriceUnit>.*) dans la ligue (?<League>.*?)", RegexOptions.Compiled),
            /* SPA */ new Regex(@"(.*)Hola, quisiera comprar tu (?<Item>.*) listado por (?<PriceAmount>\d+) (?<PriceUnit>.*) en (?<League>.*?)", RegexOptions.Compiled),
            /* KOR */ new Regex(@"(.*)안녕하세요, (?<League>.*?)에 (?<PriceAmount>\d+) (?<PriceUnit>.*)\(으\)로 올려놓은 (?<Item>.*)\(을\)를 구매하고 싶습니다(.*)", RegexOptions.Compiled),
            /* TWN */ new Regex(@"(.*)你好，我想購買 (?<Item>.*) 標價 (?<PriceAmount>\d+) (?<PriceUnit>.*) 在 (?<League>.*?)", RegexOptions.Compiled),
            /* DB  */ new Regex(@"(.*)您好，我想買在 (?<League>.*?) 的 (?<Item>.*) 價格 (?<PriceAmount>\d+) (?<PriceUnit>.*)", RegexOptions.Compiled) // TWN POEdb
        };

        /// <summary>
        /// REGEX for stash tab info <br></br>
        /// \(stash tab &quot;&quot;(?&lt;StashTab&gt;.*)&quot;&quot;; position: left (?&lt;Left&gt;\d+), top (?&lt;Top&gt;\d+)\)(?&lt;Other&gt;.*)
        /// <value>StashTab, Left, Top, Other</value>
        /// </summary>
        public static readonly Regex[] StashTabList =
        {
            /* ENG */ new Regex(@"\(stash tab ""(?<StashTab>.*)""; position: left (?<Left>\d+), top (?<Top>\d+)\)(?<Other>.*)", RegexOptions.Compiled),
            /* RUS */ new Regex(@"\(секция ""(?<StashTab>.*)""; позиция: (?<Left>\d+) столбец, (?<Top>\d+) ряд\)(?<Other>.*)", RegexOptions.Compiled),
            /* POR */ new Regex(@"\(aba do baú: ""(?<StashTab>.*)""; posição: esquerda (?<Left>\d+), topo (?<Top>\d+)\)(?<Other>.*)", RegexOptions.Compiled),
            /* THA */ new Regex(@"\(stash tab ""(?<StashTab>.*)""; ตำแหน่ง: ซ้าย (?<Left>\d+), บน (?<Top>.*)\)(?<Other>.*)", RegexOptions.Compiled), // Top position is bugged from GGG side and appears as {{TOP}) for priced items, so we use (.*) instead of (\d+)
            /* GER */ new Regex(@"\(Truhenfach ""(?<StashTab>.*)""; Position: (?<Left>\d+). von links, (?<Top>\d+). von oben\)(?<Other>.*)", RegexOptions.Compiled),
            /* FRE */ new Regex(@"\(onglet de réserve ""(?<StashTab>.*)"" \; (?<Left>\d+)e en partant de la gauche, (?<Top>\d+)e en partant du haut\)(?<Other>.*)", RegexOptions.Compiled),
            /* SPA */ new Regex(@"\(pestaña de alijo ""(?<StashTab>.*)""; posición: izquierda(?<Left>\d+), arriba (?<Top>\d+)\)(?<Other>.*)", RegexOptions.Compiled),
            /* KOR */ new Regex(@"\(보관함 탭 ""(?<StashTab>.*)"", 위치: 왼쪽 (?<Left>\d+), 상단 (?<Top>\d+)\)(?<Other>.*)", RegexOptions.Compiled),
            /* TWN */ new Regex(@"\(倉庫頁 ""(?<StashTab>.*)""; 位置: 左 (?<Left>\d+), 上 (?<Top>\d+)\)(?<Other>.*)", RegexOptions.Compiled),
            /* DB  */ new Regex(@"\[倉庫:(?<StashTab>.*) 位置: 左(?<Left>\d+), 上 (?<Top>\d+)\)(?<Other>.*)", RegexOptions.Compiled) // TWN POEdb
        };

        /// <summary>
        /// Regex for unpriced trades (excluding bulk) - will need to parse <see cref="StashTabList"/><br></br>
        /// (.*)Hi, I would like to buy your (?&lt;Item&gt;.*) in (?&lt;League&gt;.*?)\(
        /// <value>Item, League</value>
        /// </summary>
        public static readonly Regex[] UnpricedTradeList =
        {
            /* ENG */ new Regex(@"(.*)Hi, I would like to buy your (?<Item>.*) in (?<League>.*?)\(", RegexOptions.Compiled),
            /* RUS */ new Regex(@"(.*)Здравствуйте, хочу купить у вас (?<Item>.*) в лиге (?<League>.*?)", RegexOptions.Compiled),
            /* POR */ new Regex(@"(.*)Olá, eu gostaria de comprar o seu item (?<Item>.*) na (?<League>.*?)", RegexOptions.Compiled),
            /* THA */ new Regex(@"(.*)สวัสดี, เราต้องการจะชื้อของคุณ (?<Item>.*) ใน (?<League>.*?)", RegexOptions.Compiled),
            /* GER */ new Regex(@"(.*)Hi, ich möchte '(?<Item>.*)' in der '(?<League>.*?)'-Liga kaufen(.*)", RegexOptions.Compiled),
            /* FRE */ new Regex(@"(.*)Bonjour, je souhaiterais t'acheter (?<Item>.*) dans la ligue (?<League>.*?)", RegexOptions.Compiled),
            /* SPA */ new Regex(@"(.*)Hola, quisiera comprar tu (?<Item>.*) en (?<League>.*?)", RegexOptions.Compiled),
            /* KOR */ new Regex(@"(.*)안녕하세요, (?<League>.*?)에 올려놓은 (?<Item>.*)\(을\)를 구매하고 싶습니다(.*)", RegexOptions.Compiled),
            /* TWN */ new Regex(@"(.*)你好，我想購買 (?<Item>.*) 在 (?<League>.*?)", RegexOptions.Compiled),
            /* DB  */ new Regex(@"(.*)您好，我想買在 (?<League>.*?) 的 (?<Item>.*)", RegexOptions.Compiled) // TWN POEdb
        };

        /// <summary>
        /// REGEX for bulk (currency) trades - will not contain stash info<br></br>
        /// (.*)Hi, I&apos;d like to buy your (?&lt;SellAmount&gt;\d+) (?&lt;SellUnits&gt;.*?) for my (?&lt;BuyAmount&gt;\d+) (?&lt;BuyUnits&gt;.*?) in (?&lt;League&gt;.*).(?&lt;Other&gt;.*)
        /// <value>(int)SellAmount, SellUnits, (int)BuyAmount, BuyUnits, League, Other</value>
        /// </summary>
        public static readonly Regex[] BulkTradeList =
        {
            /* ENG */ new Regex(@"(.*)Hi, I'd like to buy your (?<SellAmount>\d+) (?<SellUnits>.*?) for my (?<BuyAmount>\d+) (?<BuyUnits>.*?) in (?<League>.*).(?<Other>.*)", RegexOptions.Compiled),
            /* RUS */ new Regex(@"(.*)Здравствуйте, хочу купить у вас (?<SellAmount>\d+) (?<SellUnits>.*?) за (?<BuyAmount>\d+) (?<BuyUnits>.*?) в лиге (?<League>.*).(?<Other>.*)", RegexOptions.Compiled),
            /* POR */ new Regex(@"(.*)Olá, eu gostaria de comprar seu\(s\) (?<SellAmount>\d+) (?<SellUnits>.*?) pelo\(s\) meu\(s\) (?<BuyAmount>\d+) (?<BuyUnits>.*?) na (?<League>.*).(?<Other>.*)", RegexOptions.Compiled),
            /* THA */ new Regex(@"(.*)สวัสดี เรามีความต้องการจะชื้อ (?<SellAmount>\d+) (?<SellUnits>.*?) ของคุณ ฉันมี (?<BuyAmount>\d+) (?<BuyUnits>.*?) ใน (?<League>.*).(?<Other>.*)", RegexOptions.Compiled),
            /* GER */ new Regex(@"(.*)Hi, ich möchte '(?<SellAmount>\d+) (?<SellUnits>.*?)' zum angebotenen Preis von '(?<BuyAmount>\d+) (?<BuyUnits>.*?)' in der '(?<League>.*).(?<Other>.*)'-Liga kaufen(.*)", RegexOptions.Compiled),
            /* FRE */ new Regex(@"(.*)Bonjour, je voudrais t'acheter (?<SellAmount>\d+) (?<SellUnits>.*?) contre (?<BuyAmount>\d+) (?<BuyUnits>.*?) dans la ligue (?<League>.*).(?<Other>.*)", RegexOptions.Compiled),
            /* SPA */ new Regex(@"(.*)Hola, me gustaría comprar tu\(s\) (?<SellAmount>\d+) (?<SellUnits>.*?) por mi (?<BuyAmount>\d+) (?<BuyUnits>.*?) en (?<League>.*).(?<Other>.*)", RegexOptions.Compiled),
            /* KOR */ new Regex(@"(.*)안녕하세요, (?<League>.*).?에 올려놓은(?<SellAmount>\d+) (?<SellUnits>.*?)\(을\)를 제 (?<BuyAmount>\d+) (?<BuyUnits>.*?)\(으\)로 구매하고 싶습니다(?<Other>.*)", RegexOptions.Compiled),
            /* TWN */ new Regex(@"(.*)你好，我想用 (?<SellAmount>\d+) (?<SellUnits>.*?) 購買 (?<BuyAmount>\d+) (?<BuyUnits>.*?) in (?<League>.*).(?<Other>.*)", RegexOptions.Compiled),
            /* DB  */ new Regex(@"(.*)您好，我想買在 (?<League>.*).? 的 (?<SellAmount>\d+) (?<SellUnits>.*?) 個 (.*) 直購價 (?<BuyAmount>\d+) (?<BuyUnits>.*(?<Other>.*))", RegexOptions.Compiled) // POEdb
        };

        /// <summary>
        /// Regex if player joins an area <br></br>
        /// (.*) : (?&lt;Player&gt;.*?) has joined the area.*
        /// <value>Player</value>
        /// </summary>
        public static readonly Regex[] AreaJoinedList =
        {
            /* ENG */ new Regex(@"(.*) : (?<Player>.*?) has joined the area.*", RegexOptions.Compiled),
            /* FRE */ new Regex(@"(.*) : (?<Player>.*?) a rejoint la zone.*", RegexOptions.Compiled),
            /* GER */ new Regex(@"(.*) : (?<Player>.*?) hat das Gebiet betreten.*", RegexOptions.Compiled),
            /* POR */ new Regex(@"(.*) : (?<Player>.*?) entrou na área.*", RegexOptions.Compiled),
            /* RUS */ new Regex(@"(.*) : (?<Player>.*?) присоединился.*", RegexOptions.Compiled),
            /* THA */ new Regex(@"(.*) : (?<Player>.*?) เข้าสู่พื้นที่.*", RegexOptions.Compiled),
            /* SPA */ new Regex(@"(.*) : (?<Player>.*?) se unió al área.*", RegexOptions.Compiled),
            /* KOR */ new Regex(@"(.*) : (?<Player>.*?)(이)가 구역에 들어왔습니다.*", RegexOptions.Compiled),
            /* TWN */ new Regex(@"(.*) : (?<Player>.*?) 進入了此區域.*", RegexOptions.Compiled)
        };

        /// <summary>
        /// Regex if a player leaves an area <br></br>
        /// (.*) : (?&lt;Player&gt;.*?) has left the area.*
        /// <value>Player</value>
        /// </summary>
        public static readonly Regex[] AreaLeftList =
        {
            /* ENG */ new Regex(@"(.*) : (?<Player>.*?) has left the area.*", RegexOptions.Compiled),
            /* FRE */ new Regex(@"(.*) : (?<Player>.*?) a quitté la zone.*", RegexOptions.Compiled),
            /* GER */ new Regex(@"(.*) : (?<Player>.*?) hat das Gebiet verlassen.*", RegexOptions.Compiled),
            /* POR */ new Regex(@"(.*) : (?<Player>.*?) saiu da área.*", RegexOptions.Compiled),
            /* RUS */ new Regex(@"(.*) : (?<Player>.*?) покинул область.*", RegexOptions.Compiled),
            /* THA */ new Regex(@"(.*) : (?<Player>.*?) ออกจากพื้นที่.*", RegexOptions.Compiled),
            /* SPA */ new Regex(@"(.*) : (?<Player>.*?) abandonó el área.*", RegexOptions.Compiled),
            /* KOR */ new Regex(@"(.*) : (?<Player>.*?)(이)가 구역에서 나갔습니다.*", RegexOptions.Compiled),
            /* TWN */ new Regex(@"(.*) : (?<Player>.*?) 離開了此區域.*", RegexOptions.Compiled)
        };

        /// <summary>
        /// Regex if you enter an area <br></br>
        /// (.*) : You have entered (?&lt;Area&gt;.*?).
        /// <value>Area</value>
        /// </summary>
        public static readonly Regex[] YouJoinList =
        {
            /* ENG */ new Regex(@"(.*) : You have entered (?<Area>.*?).", RegexOptions.Compiled)
            // TODO add language specific regex
        };
        
        // TODO add afk regex
    }
}