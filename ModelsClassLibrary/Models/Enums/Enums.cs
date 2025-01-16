using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsClassLibrary.Models.Enums
{
    public enum CategoryEnum
    {
        All = 0,
        AttaRiceOilAndDals,
        ColdDrinksAndJuices,
        DairyBreadAndEggs,
        FrozenFoodAndIceCreams,
        FruitsAndVegetables,
        MasalaAndDryFruits,
        PackageFood,
        SweetCravings
    }

    public enum GenderEnum
    {
        Male,
        Female,
        Others
    }

    public enum UserTypeEnum
    {
        Customer,
        Admin
    }

}
