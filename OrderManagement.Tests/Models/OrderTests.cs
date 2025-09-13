using FluentAssertions;
using OrderManagementSystem.Enums;
using OrderManagementSystem.Models;

namespace OrderManagement.Tests.Models;

public class OrderTests
{
    [Fact]
    public void Order_ShouldHaveDefaultValues_WhenCreated()
    {
        var order = new OrderModel();

        order.Id.Should().NotBeEmpty();
        order.Client.Should().BeEmpty();
        order.Product.Should().BeEmpty();
        order.Value.Should().Be(0);
        order.Status.Should().Be(OrderStatusEnum.Pendente);
        order.CreationDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}