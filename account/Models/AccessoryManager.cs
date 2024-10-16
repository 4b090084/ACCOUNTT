using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace account.Models
{
    class AccessoryManager
    {
        private static AccessoryManager _instance;
        public static AccessoryManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AccessoryManager();
                }
                return _instance;
            }
        }

        private List<string> ownedAccessories;
        public string CurrentAccessory { get; set; }

        private AccessoryManager()
        {
            ownedAccessories = new List<string>();
        }

        public void AddAccessory(string accessoryName)
        {
            if (!ownedAccessories.Contains(accessoryName))
            {
                ownedAccessories.Add(accessoryName);
            }
            CurrentAccessory = accessoryName;
        }

        public bool HasAccessory(string accessoryName)
        {
            return ownedAccessories.Contains(accessoryName);
        }

        public List<string> GetOwnedAccessories()
        {
            return new List<string>(ownedAccessories);
        }

        public void EquipAccessory(string accessoryName)
        {
            if (HasAccessory(accessoryName))
            {
                CurrentAccessory = accessoryName;
            }
        }

        public void RemoveCurrentAccessory()
        {
            CurrentAccessory = null;
        }

        public string AccessoryImage { get; set; }
    }
}
