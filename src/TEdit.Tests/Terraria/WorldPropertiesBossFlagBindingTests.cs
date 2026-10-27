using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using TEdit.Terraria;
using TEdit.View;
using Xunit;

namespace TEdit.Tests.Terraria;

public class WorldPropertiesBossFlagBindingTests
{
    [Fact]
    public void DukeFishronAndFlyingDutchmanCheckboxesUseIndependentWorldFlags()
    {
        Exception? testException = null;
        var testThread = new Thread(() =>
        {
            try
            {
                var world = new World
                {
                    DownedFishron = true,
                    DownedPirates = false
                };
                var view = new WorldPropertiesView
                {
                    DataContext = new { CurrentWorld = world }
                };

                var dukeFishron = Assert.IsType<CheckBox>(view.FindName("DukeFishronDownedCheckBox"));
                var flyingDutchman = Assert.IsType<CheckBox>(view.FindName("FlyingDutchmanDownedCheckBox"));
                var worldContext = new { CurrentWorld = world };
                var fishronBindingDefinition = BindingOperations.GetBinding(dukeFishron, ToggleButton.IsCheckedProperty)!;
                var dutchmanBindingDefinition = BindingOperations.GetBinding(flyingDutchman, ToggleButton.IsCheckedProperty)!;
                dukeFishron.SetBinding(ToggleButton.IsCheckedProperty, new Binding(fishronBindingDefinition.Path.Path)
                {
                    Source = worldContext,
                    Mode = fishronBindingDefinition.Mode
                });
                flyingDutchman.SetBinding(ToggleButton.IsCheckedProperty, new Binding(dutchmanBindingDefinition.Path.Path)
                {
                    Source = worldContext,
                    Mode = dutchmanBindingDefinition.Mode
                });
                var fishronBinding = BindingOperations.GetBindingExpression(dukeFishron, ToggleButton.IsCheckedProperty)!;
                var dutchmanBinding = BindingOperations.GetBindingExpression(flyingDutchman, ToggleButton.IsCheckedProperty)!;

                fishronBinding.UpdateTarget();
                dutchmanBinding.UpdateTarget();
                Assert.True(dukeFishron.IsChecked);
                Assert.False(flyingDutchman.IsChecked);

                flyingDutchman.IsChecked = true;
                dutchmanBinding.UpdateSource();
                Assert.True(world.DownedPirates);
                Assert.True(world.DownedFishron);

                dukeFishron.IsChecked = false;
                fishronBinding.UpdateSource();
                Assert.False(world.DownedFishron);
                Assert.True(world.DownedPirates);
            }
            catch (Exception exception)
            {
                testException = exception;
            }
        });

        testThread.SetApartmentState(ApartmentState.STA);
        testThread.Start();
        testThread.Join();

        if (testException is not null)
        {
            ExceptionDispatchInfo.Capture(testException).Throw();
        }
    }
}
