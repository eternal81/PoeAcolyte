using System.Collections.Generic;
using System.Drawing;
using PoeAcolyte.DataTypes;
using PoeAcolyte.Resources;
using Region = PoeAcolyte.Resources.Region;
// ReSharper disable StringLiteralTypo

namespace PoeAcolyte.Helpers
{
    public static class PoeConverter
    {
        private static readonly Dictionary<string, Image> StringPriceToImage = new()
        {
            {"c", CurrencyImages.CurrencyRerollRare},
            {"chaos", CurrencyImages.CurrencyRerollRare},
            {"chaos orb", CurrencyImages.CurrencyRerollRare},

            {"alt", CurrencyImages.CurrencyRerollMagic},
            {"alteration", CurrencyImages.CurrencyRerollMagic},
            {"orb of alteration", CurrencyImages.CurrencyRerollMagic},
            {"alts", CurrencyImages.CurrencyRerollMagic},

            {"alch", CurrencyImages.CurrencyUpgradeToRare},
            {"orb of alchemy", CurrencyImages.CurrencyUpgradeToRare},
            {"alchs", CurrencyImages.CurrencyUpgradeToRare},

            {"fuse", CurrencyImages.CurrencyRerollSocketLinks},
            {"fus", CurrencyImages.CurrencyRerollSocketLinks},
            {"fusing", CurrencyImages.CurrencyRerollSocketLinks},
            {"orb of fusing", CurrencyImages.CurrencyRerollSocketLinks},

            {"gcp", CurrencyImages.CurrencyGemQuality},
            {"gem cutters prism", CurrencyImages.CurrencyGemQuality},
            {"gem cutter's prism", CurrencyImages.CurrencyGemQuality},
            {"gemcutters prism", CurrencyImages.CurrencyGemQuality},
            {"gemcutter's prism", CurrencyImages.CurrencyGemQuality},

            {"ex", CurrencyImages.CurrencyAddModToRare},
            {"exa", CurrencyImages.CurrencyAddModToRare},
            {"exalt", CurrencyImages.CurrencyAddModToRare},
            {"exalts", CurrencyImages.CurrencyAddModToRare},
            {"exalted", CurrencyImages.CurrencyAddModToRare},
            {"exalted orb", CurrencyImages.CurrencyAddModToRare},

            {"chrome", CurrencyImages.CurrencyRerollSocketColours},
            {"chromes", CurrencyImages.CurrencyRerollSocketColours},
            {"chromatic", CurrencyImages.CurrencyRerollSocketColours},
            {"chromatics", CurrencyImages.CurrencyRerollSocketColours},
            {"chromatic orb", CurrencyImages.CurrencyRerollSocketColours},

            {"jewel", CurrencyImages.CurrencyRerollSocketNumbers},
            {"jewellers", CurrencyImages.CurrencyRerollSocketNumbers},
            {"jewellers orb", CurrencyImages.CurrencyRerollSocketNumbers},
            {"jeweller's orb", CurrencyImages.CurrencyRerollSocketNumbers},

            {"chance", CurrencyImages.CurrencyUpgradeRandomly},
            {"chances", CurrencyImages.CurrencyUpgradeRandomly},
            {"chance orb", CurrencyImages.CurrencyUpgradeRandomly},
            {"orb of chance", CurrencyImages.CurrencyUpgradeRandomly},

            {"chis", CurrencyImages.CurrencyMapQuality},
            {"chisel", CurrencyImages.CurrencyMapQuality},
            {"chisels", CurrencyImages.CurrencyMapQuality},
            {"cartographer", CurrencyImages.CurrencyMapQuality},
            {"cartographers", CurrencyImages.CurrencyMapQuality},
            {"cartographer's chisel", CurrencyImages.CurrencyMapQuality},

            {"scour", CurrencyImages.CurrencyConvertToNormal},
            {"scouring", CurrencyImages.CurrencyConvertToNormal},
            {"orb of scouring", CurrencyImages.CurrencyConvertToNormal},

            {"regret", CurrencyImages.CurrencyPassiveSkillRefund},
            {"regrets", CurrencyImages.CurrencyPassiveSkillRefund},
            {"orb of regret", CurrencyImages.CurrencyPassiveSkillRefund},

            {"reg", CurrencyImages.CurrencyUpgradeMagicToRare},
            {"regal", CurrencyImages.CurrencyUpgradeMagicToRare},
            {"regals", CurrencyImages.CurrencyUpgradeMagicToRare},
            {"regal orb", CurrencyImages.CurrencyUpgradeMagicToRare},

            {"div", CurrencyImages.CurrencyRerollRare},
            {"divs", CurrencyImages.CurrencyRerollRare},
            {"divine", CurrencyImages.CurrencyRerollRare},
            {"divine orb", CurrencyImages.CurrencyRerollRare},

            {"bless", CurrencyImages.CurrencyImplicitMod},
            {"blessed", CurrencyImages.CurrencyImplicitMod},
            {"blessed orb", CurrencyImages.CurrencyImplicitMod},

            {"vaal", CurrencyImages.CurrencyVaal},
            {"vaals", CurrencyImages.CurrencyVaal},
            {"vaal orb", CurrencyImages.CurrencyVaal},

            {"annul", CurrencyImages.AnnullOrb},
            {"annuls", CurrencyImages.AnnullOrb},
            {"orb of annulment", CurrencyImages.AnnullOrb},

            {"ancient", CurrencyImages.AncientOrb},
            {"ancients", CurrencyImages.AncientOrb},
            {"ancient orb", CurrencyImages.AncientOrb},

            {"horizon", CurrencyImages.HorizonOrb},
            {"horizon orb", CurrencyImages.HorizonOrb},
            {"orb of horizons", CurrencyImages.HorizonOrb},
            {"horizons", CurrencyImages.HorizonOrb},

            {"orb of transmutation", CurrencyImages.CurrencyUpgradeToMagic},
            {"trans", CurrencyImages.CurrencyUpgradeToMagic},
            {"transmute", CurrencyImages.CurrencyUpgradeToMagic},
            {"transmutes", CurrencyImages.CurrencyUpgradeToMagic},

            {"mir", CurrencyImages.CurrencyDuplicate},
            {"mirror", CurrencyImages.CurrencyDuplicate},
            {"mirror of kalandra", CurrencyImages.CurrencyDuplicate},

            {"aug", CurrencyImages.CurrencyAddModToMagic},
            {"augs", CurrencyImages.CurrencyAddModToMagic},
            {"orb of augmentation", CurrencyImages.CurrencyAddModToMagic},

            {"stacked deck", CurrencyImages.Deck},
            {"perandus coin", CurrencyImages.CurrencyCoin},
            {"harbinger's orb", CurrencyImages.HarbingerOrb},
            {"wisdom", CurrencyImages.CurrencyIdentification},
            {"scroll of wisdom", CurrencyImages.CurrencyIdentification},
            {"portal", CurrencyImages.CurrencyPortal},
            {"portal scroll", CurrencyImages.CurrencyPortal},
            {"armourer's scrap", CurrencyImages.CurrencyArmourQuality},
            {"blacksmith's whetstone", CurrencyImages.CurrencyWeaponQuality},
            {"glassblower's bauble", CurrencyImages.CurrencyFlaskQuality},
            {"eternal orb", CurrencyImages.CurrencyImprintOrb},
            {"rogue's marker", CurrencyImages.HeistCoinCurrency},
            {"silver coin", CurrencyImages.SilverObol},
            {"crusader's exalted orb", CurrencyImages.CrusaderOrb},
            {"redeemer's exalted orb", CurrencyImages.EyrieOrb},
            {"hunter's exalted orb", CurrencyImages.BasiliskOrb},
            {"warlord's exalted orb", CurrencyImages.ConquerorOrb},
            {"awakener's orb", CurrencyImages.TransferOrb},
            {"maven's orb", CurrencyImages.MavenOrb},
            {"facetor's lens", CurrencyImages.CurrencyGemExperience},
            {"prime regrading lens", CurrencyImages.AlternateSkillGemCurrency},
            {"secondary regrading lens", CurrencyImages.AlternateSupportGemCurrency},
            {"tempering orb", CurrencyImages.DivineEnchantBodyArmourCurrency},
            {"tailoring orb", CurrencyImages.DivineEnchantWeaponCurrency},
            {"ritual vessel", CurrencyImages.Effigy},
            {"simple sextant", CurrencyImages.AtlasRadiusTier1},
            {"prime sextant", CurrencyImages.AtlasRadiusTier2},
            {"awakened sextant", CurrencyImages.AtlasRadiusTier3},
            {"elevated sextant", CurrencyImages.AtlasRadiusTier4},
            {"orb of unmaking", CurrencyImages.RegretOrb},
            {"veiled chaos orb", CurrencyImages.VeiledChaosOrb}
        };

        public static Image FromPriceString(string input)
        {
            input = input is null ? string.Empty : input.ToLower();
            return StringPriceToImage.ContainsKey(input) ? StringPriceToImage[input] : CurrencyImages.logo;
        }

        public static string GetEntryDescription(PoeLogEntry entry)
        {
            switch (entry.PoeLogEntryType)
            {
                case IPoeLogEntry.PoeLogEntryTypeEnum.BulkTrade:
                    return $"{entry.BuyPriceAmount} {entry.BuyPriceUnits}\r\n" +
                           $"{entry.Player} ({entry.PoeLogEntryType})\r\n" +
                           $"{entry.League}";
                case IPoeLogEntry.PoeLogEntryTypeEnum.PricedTrade:
                    return $"{entry.Item}\r\n" +
                           $"({entry.StashTab})\r\n" +
                           $"Top: {entry.Top}, Left: {entry.Left}\r\n" +
                           $"{entry.Player} ({entry.PoeLogEntryType})" +
                           $"{entry.League}";
                case IPoeLogEntry.PoeLogEntryTypeEnum.UnpricedTrade:
                    return $"{entry.Item}\r\n" +
                           $"{entry.Player} ({entry.PoeLogEntryType} )\r\n" +
                           $"{entry.League}";
                default:
                    return "";
            }
        }
    }
}