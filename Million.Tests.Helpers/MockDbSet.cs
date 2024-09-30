using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;

public static class DbSetMockExtensions
{
    public static Mock<DbSet<T>> CreateMockDbSet<T>(this IList<T> data) where T : class
    {
        var queryableData = data.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();

        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());

        mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(data.Add);
        mockSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>(t => data.Remove(t));
        mockSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(t =>
        {
            foreach (var item in t)
            {
                data.Remove(item);
            }
        });

        return mockSet;
    }
}