using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grind.Common
{
    /// <summary>
    /// http://d3advanced.com/wiki/index.php?title=List_of_Common_UI_Elements
    /// </summary>
    public enum D3UIButton : ulong
    {

        /// <summary>
        /// 0x1AE2209980AAEA6
        /// </summary>
        SelectQuestButton = 0x1AE2209980AAEA6,  // 2BBDBAC4: 1AE2209980AAEA69 Root.NormalLayer.BattleNetQuestSelection_main.LayoutRoot.OverlayContainer.SelectQuestButton

        /// <summary>
        /// 0xB4433DA3F648A992
        /// </summary>
        OkButton = 0xB4433DA3F648A992, // 2440DBEC: B4433DA3F648A992 Root.TopLayer.BattleNetModalNotifications_main.ModalNotification.Buttons.ButtonList.OkButton (Visible: True)    

        /// <summary>
        /// 0x5DB09161C4D6B4C6
        /// </summary>
        ExitButton = 0x5DB09161C4D6B4C6,

        /// <summary>
        /// 0xBFAAF48BA9316742
        /// </summary>
        ReviveButton = 0xBFAAF48BA9316742,

        /// <summary>
        /// 0x80F5D06A035848A5
        /// </summary>
        RepairAll = 0x80F5D06A035848A5,

        /// <summary>
        /// 0x2289FE26DA955A81
        /// </summary>
        SkipVideo = 0x2289FE26DA955A81,

        /// <summary>
        /// 0x891D21408238D18E
        /// </summary>
        ConfirmSkipVideo = 0x891D21408238D18E,

        /// <summary>
        /// 0x942F41B6B5346714
        /// </summary>
        SkipConversationPCtoNPC = 0x942F41B6B5346714,

        /// <summary>
        /// 0xC06278A08ADCF3AA
        /// </summary>
        SkipConversationNPCtoNPC = 0xC06278A08ADCF3AA,

        /// <summary>
        /// 0xE1F43DD874E42728
        /// </summary>
        DrinkPotion = 0xE1F43DD874E42728,

        /// <summary>
        /// 0x51A3923949DC80B7
        /// </summary>
        StartGame = 0x51A3923949DC80B7,

        /// <summary>
        /// 0xC4A9CC94C0A929B
        /// </summary>
        ChangeQuest = 0xC4A9CC94C0A929B,

        /// <summary>
        /// 0x1AE2209980AAEA69
        /// </summary>
        SelectQuest = 0x1AE2209980AAEA69,

        /// <summary>
        /// 0x368FF8C552241695
        /// </summary>
        CloseInventory = 0x368FF8C552241695,

        /// <summary>
        /// 0xBD8B3C3679D4F4D9
        /// </summary>
        RepairIndicator = 0xBD8B3C3679D4F4D9,

        /// <summary>
        /// 0x48BA183D534400AD
        /// 0002FA62: 00000001(00000000) # {c:ffffffff}Salvage{/c} = 0x1D19D9E
        /// C3024CC: 48BA183D534400AD Root.NormalLayer.vendor_dialog_mainPage.salvage_dialog.salvage_button (Visible: True)
        /// </summary>
        Salvage = 0x48BA183D534400AD

    }

}
