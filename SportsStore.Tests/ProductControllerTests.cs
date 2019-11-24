using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SportsStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

// ��� ����� 
//Can_Paginate
//Can_Send_Pagination_View_Model
//Can_Filter_Products

namespace SportsStore.Tests
{
    // ��������� ������������ �������� ��������� �� ��������
    #region �������� �����
    // ��������� ������������ �������� ��������� �� �������� ����� ��������, ������
    // ������������� ���������, ������� ��� � ����������� ������ ProductController
    // � ������ ����� List() , ����� ����������� ���������� ��������. ����� ����������
    // ������� Products ����� �������� � ����, ������� ��������� �� �������� ������ � ���
    // ���������� ����������.

    // ��������� ������, ������������ �� ������ ��������, �������� ��������� ��������.
    // ����������� �������� ������ ViewResult, � �������� ��� �������� ViewData.Model
    // ������ ���� ��������� � ���������� ���� ������.
    #endregion
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Products[]
                {
                new Products {ProductID = 1, Name = "P1" },
                new Products {ProductID = 2, Name = "P2" },
                new Products {ProductID = 3, Name = "P3" },
                new Products {ProductID = 4, Name = "P4" },
                new Products {ProductID = 5, Name = "P5" }
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // ��������
            //IEnumerable<Products> result = controller.List(2).ViewData.Model as IEnumerable<Products>;
            ProductsListViewModel result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;
            // 2 - ��� page ��� ������� �������� ������ ������ �������� (��������������� � ����� List) �� ����� 
            // ����� � ������ �� ������� Equel ��������� ��� Name P4 � P5 ��� ��������� �������� ������, 
            // ��� ����� �� ����������� ���������� �������� (� ����� ����� ������) � ������� ���������� ������ 
            // �� ����. 
            // �������� null ��� category - List(null, 2), �� �������� ��� ������� Products, ������� �������
            // ��� ��������� �� ���������, ��� ������������� �������� ����� ����������� ���� �
            // �� ���������.

            //ViewData ������������ ������� �� ��� ����-��������
            //�����������
            Products[] prodArray = result.Products.ToArray(); // � ���� ������ prodArray (��� ������ ��������) 
            //����� ��������� P4 � P5 ��� ��� �� ������ ������� � PageSize = 3 ��� ������� P1 P2 P3 

            Assert.True(prodArray.Length == 2); // ������ ���-�� ��������� � ������� ���������� 2 ProductID 4 � 5
            Assert.Equal("P4", prodArray[0].Name); // �������� ������ ���� ������� � ������� �������� �������
            Assert.Equal("P5", prodArray[1].Name);

        }

        #region Can_Send_Pagination_View_Model
        //��� ���������� �������������� � ���, ��� ���������� ���������� ������������� ������
        //���� ����� ����� � ��������� �� ��������. �� �� ������� ��������� ����, ����������� �
        //����� ProductControllerTests ������ ��������� �������, ������� ������������ �����
        //��������: 
        #endregion
        //-----------------------------------------------------
        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Products[]
                {
                new Products {ProductID = 1, Name = "P1" },
                new Products {ProductID = 2, Name = "P2" },
                new Products {ProductID = 3, Name = "P3" },
                new Products {ProductID = 4, Name = "P4" },
                new Products {ProductID = 5, Name = "P5" }
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // ��������
            ProductsListViewModel result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;
            // ����� ������������� ������ ������� ��� ������ � ����� List 

            //�����������
            PagingInfo pageInfo = result.PagingInfo; // ������ ���������� ������ �� ������ PagingInfo
            Assert.Equal(2, pageInfo.CurrentPage); // ������� �������� ������ ���� ������ ��� ��� �� ���������� List(2) 
            Assert.Equal(3, pageInfo.ItemsPerPage); // �������� PageSize = 3 ������ ���-�� ����������� ������� �� �������� 
            Assert.Equal(5, pageInfo.TotalItems); // ������ ������ ���-�� ������� - 5
            Assert.Equal(2, pageInfo.TotalPages); // ������ ������ ���-�� ������� - 2

        }
        //-----------------------------------------------------
        [Fact]
        public void Can_Filter_Products()
        {
            #region �������� 
            //���� ���� ������� ������������� ���������, ���������� ������� Products, ������� ��
            //������� � ��������� ���������. � �������������� ������ �������� List() �������������
            //���� ������������� ���������, � ���������� ����������� �� ������� ������� ����������
            //�������� � ���������� �������. 
            #endregion

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Products[]
                {
                    new Products { ProductID=1, Name="P1", Category="Cat1"},
                    new Products { ProductID=2, Name="P2", Category="Cat2"},
                    new Products { ProductID=3, Name="P3", Category="Cat1"},
                    new Products { ProductID=4, Name="P4", Category="Cat2"},
                    new Products { ProductID=5, Name="P5", Category="Cat3"},
                });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            Products[] result =
                (controller.List("Cat2", 1).ViewData.Model as ProductsListViewModel).Products.ToArray();

            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2"); // ������ ��������� � ��� ���� Name �� ������� P2-Cat2 P4-Cat2 
            Assert.True(result[1].Name == "P4" && result[0].Category == "Cat2");

        }
        //-------------------------------------------------------------------------

        // ������� ������� ������������ ���������

        //�������������� ����������� ��������� ���������� ��������� ������� ��� ���������
        //��������� ����� ������. �� �������� ������������� ���������, ������� �������� ��
        //������� ������ � ������������ ��������� ���������, � ����� ������� ����� ��������
        //List(), ���������� ������ ��������� �� �������.

        //� ����� ����� ���������� ����� List() ��� ��������
        //���������, ����� �������������� � ������������ �������� ������ ���������� �������.

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Products[]
                {
                    new Products { ProductID=1, Name="P1", Category="Cat1"}, 
                    new Products { ProductID=2, Name="P2", Category="Cat2"},
                    new Products { ProductID=3, Name="P3", Category="Cat1"},
                    new Products { ProductID=4, Name="P4", Category="Cat2"},
                    new Products { ProductID=5, Name="P5", Category="Cat3"},
                });

            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3; 

            Func<ViewResult, ProductsListViewModel> GetModel = result1 => result1?.ViewData?.Model as ProductsListViewModel;

            int? res1 = GetModel(target.List("Cat1"))?.PagingInfo.TotalItems; 
            int? res2 = GetModel(target.List("Cat2"))?.PagingInfo.TotalItems;
            int? res3 = GetModel(target.List("Cat3"))?.PagingInfo.TotalItems;
            int? resAll = GetModel(target.List(null))?.PagingInfo.TotalItems;

            Assert.Equal(2, res1); // Cat1 ������ ���� 2 ���������
            Assert.Equal(2, res2); // Cat2 ������ ���� 2 ���������
            Assert.Equal(1, res3); // Cat1 ������ ���� 1 ���������
            Assert.Equal(5, resAll); // ����� ��������� ������ ���� 5 

        }

    }
}