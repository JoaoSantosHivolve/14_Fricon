using System.Collections.Generic;
using Assets.Scripts.Common;

namespace FridgeScripts
{
    public class FridgeManager : Singleton<FridgeManager>
    {
        public List<Fridge> fridges;
        public Fridge selectedFridge;
    }
}