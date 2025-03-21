using System;
using System.Collections.Generic;

// Interface meant to guarantee that any and every SettingsCategory has a way to apply and revert changes made by the user.
// Meant primarily for compat between SettingsMenu having "Apply" and "Reset" buttons and the specific SettingsCategory
//   being viewed.
interface ISettingsCategory {
    public void ApplyChanges();

    public void RevertToDefaults();
}