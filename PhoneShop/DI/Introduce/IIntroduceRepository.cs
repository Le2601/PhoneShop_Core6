﻿using PhoneShop.Areas.Admin.Data;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.Introduce
{
    public interface IIntroduceRepository
    {
        void Update(IntroduceData model);

        public List<IntroduceViewModel> GetIntroduce();

    }
}
