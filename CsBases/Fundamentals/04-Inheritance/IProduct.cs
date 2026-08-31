using System;

namespace CsBases.Fundamentals;

public interface IProduct
{
    void ApplyDiscount(decimal percentage);
    string GetDescription();
}
