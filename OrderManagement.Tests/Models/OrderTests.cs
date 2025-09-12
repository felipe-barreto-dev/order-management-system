using FluentAssertions;
using order_management_system.Enums;

namespace OrderManagement.Tests.Models;

public class OrderTests
{
    [Fact]
    public void Order_ShouldHaveDefaultValues_WhenCreated()
    {
        var order = new Order();

        order.Id.Should().NotBeEmpty();
        order.Cliente.Should().BeEmpty();
        order.Produto.Should().BeEmpty();
        order.Valor.Should().Be(0);
        order.Status.Should().Be(OrderStatusEnum.Pendente);
        order.DataCriacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}