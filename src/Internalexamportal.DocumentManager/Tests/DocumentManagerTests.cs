using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Internalexamportal.DocumentManager.Core;
using Xunit;

namespace Internalexamportal.DocumentManager.Tests
{
    // These tests are written to demonstrate sample codes only. All 
    // these test are testing there was no exception at template building  
    // process this is not enough test to gurantee functional codes
    public class DocumentManagerTests
    {
        public DocumentManagerTests()
        {
            new Aspose.Words.License().SetLicense("Aspose.Total.lic");
        }

        [Fact]
        public void InjectShouldNotThrowException()
        {
            try
            {
                //Arrange And Act
                FluentAspose.TakeDocument("InjectTest.docx")
                    .Inject(new
                    {
                        FirstName = "Manoj",
                        LastName = "Gaikwad",
                        Gender = Gender.Male,
                        Salary = 11111111111,
                        Married = false,
                        Company = "Vidhyaos",
                        ContactPhone = "7276899988",
                        ContactEmail = "manojdgaikwad4165@gmail.com",
                        Date = new DateTime(1997, 07, 11)
                    })
                    .Render()
                    .CleanUp()
                    .Save("output.docx");
            }
            catch (Exception ex)
            {
                //Assert
                Assert.True(false, "Expected no exception, but got: " + ex.Message);
            }
        }

        [Fact]
        public void InjectFieldsShouldNotThrowException()
        {
            try
            {
                //Arrange And Act
                Core.FluentAspose.TakeDocument("FieldTest.docx")
                    .InjectField("FullName").WithValue("James Bond")
                    .InjectField("Company") .WithValue("MI5 Headquarter")
                    .InjectField("Address") .WithValue("Milbank")
                    .InjectField("City")    .WithValue("London")
                    .Render()
                    .Save("FieldTest.out.docx");
            }
            catch (Exception ex)
            {
                //Assert
                Assert.True(false, "Expected no exception, but got: " + ex.Message);
            }
        }

        [Fact]
        public void CleanUpShouldNotThrowException()
        {
            try
            {
                //Arrange And Act
                Core.FluentAspose.TakeDocument("FieldTest.docx")
                    .InjectField("FullName").WithValue("James Bond")
                    .Render()
                    .CleanUp()
                    .Save("FieldTest.out.docx");
            }
            catch (Exception ex)
            {
                //Assert
                Assert.True(false, "Expected no exception, but got: " + ex.Message);
            }
        }

        [Fact]
        public void InjectBookmarksShouldNotThrowException()
        {
            try
            {
                //Arrange And Act
                Core.FluentAspose.TakeDocument("BookmarkTest.docx")
                    .InjectBookmark("FullName")    .WithText("James Bond")
                    .InjectBookmark("Company")     .WithText("MI5 Headquarter")
                    .InjectBookmark("Address")     .WithText("Milbank")
                    .InjectBookmark("City")        .WithText("London")
                    .InjectBookmark("ProfileImage").WithImage("ProfileImage.jpg")
                    .Render()
                    .Save("BookmarkTest.pdf");
            }
            catch (Exception ex)
            {
                //Assert
                Assert.True(false, "Expected no exception, but got: " + ex.Message);
            }
        }

        [Fact]
        public void AssociateDataSourceShouldNotThrowException()
        {
            try
            {
                //Arrange
                var vendors = GetVendors();

                //Act
                Core.FluentAspose.TakeDocument("VendorTemplate.doc")
                    .AssociateDataSource(vendors).WithMapper(new VendorXmlMapper())
                    .Render()
                    .Save("MailMergeUsingMustacheSyntax_out.docx");
            }
            catch (Exception ex)
            {
                //Assert
                Assert.True(false, "Expected no exception, but got: " + ex.Message);
            }
        }

        [Fact]
        public void AssociateDataSourceWithMapperShouldNotThrowException()
        {
            try
            {
                //Arrange
                var vendors = GetVendors();

                //Act
                Core.FluentAspose.TakeDocument("VendorTemplate.doc")
                    .AssociateDataSource(vendors).WithMapper<VendorXmlMapper>()
                    .Render()
                    .Save("MailMergeUsingMustacheSyntax_out.docx");
            }
            catch (Exception ex)
            {
                //Assert
                Assert.True(false, "Expected no exception, but got: " + ex.Message);
            }
        }

        public enum Gender
        {
            Male,
            Female
        }

        private static List<Vendor> GetVendors()
        {
            return new List<Vendor>()
            {
                new Vendor()
                {
                    Name = "Toyota",
                    Products = new List<Product>()
                    {
                        new Product()
                        {
                            Name = "Corolla GLI",
                            Description = "GLI Description",
                            Price = 18
                        },
                        new Product()
                        {
                            Name = "Corolla XLI",
                            Description = "XLI Description",
                            Price = 16
                        }
                    }
                },
                new Vendor()
                {
                    Name = "Honda",
                    Products = new List<Product>()
                    {
                        new Product()
                        {
                            Name = "City",
                            Description = "City Description",
                            Price = 15
                        },
                        new Product()
                        {
                            Name = "Civic",
                            Description = "Civic Description",
                            Price = 18
                        }
                    }
                },
            };
        }

        public class Vendor
        {
            public string Name { get; set; }

            public List<Product> Products { get; set; }
        }

        public class Product
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
        }

        public class VendorXmlMapper : IXmlMapper<IList<Vendor>>
        {
            public XDocument MapToXDocument(IList<Vendor> vendors)
            {
                var xDocument = new XDocument(
                    new XElement("Vendors",
                        vendors.Select(vendor =>
                            new XElement("Vendor",
                                new XElement("Name", vendor.Name),
                                vendor.Products.Select(product =>
                                    new XElement("Product",
                                        new XElement("Name", product.Name),
                                        new XElement("Description", product.Description),
                                        new XElement("Price", product.Price)
                                    )
                                )
                            )
                        )
                    )
                );

                return xDocument;
            }
        }
    }
}
