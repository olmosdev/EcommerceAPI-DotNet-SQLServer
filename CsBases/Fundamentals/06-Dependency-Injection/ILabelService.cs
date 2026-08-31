using System;

namespace CsBases.Fundamentals._06_Dependency_Injection;

public interface ILabelService
{
    string GenerateLabel(Product product);
}
