using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Stationery.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    OrderStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    StockQuantity = table.Column<int>(type: "integer", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    BrandId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CartId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Faber-Castell" },
                    { 2, "Rotring" },
                    { 3, "Stabilo" },
                    { 4, "Adel" },
                    { 5, "Bic" },
                    { 6, "Gıpta" },
                    { 7, "Moleskine" },
                    { 8, "Keskin Color" },
                    { 9, "Carioca" },
                    { 10, "Limit Yayınları" },
                    { 11, "Bilgi Sarmal" },
                    { 12, "3D Yayınları" },
                    { 13, "Çap Yayınları" },
                    { 14, "Hız Yayınları" },
                    { 15, "Palme Yayınevi" },
                    { 16, "Tonguç Akademi" },
                    { 17, "MAS" },
                    { 18, "Maped" },
                    { 19, "Pritt" },
                    { 20, "Milan" },
                    { 21, "Navigator" },
                    { 22, "Mopak" },
                    { 23, "Ve-Ge" },
                    { 24, "Eastpak" },
                    { 25, "Noki" },
                    { 26, "Esselte" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Uçlu, tükenmez, kurşun vb.", "Kalemler" },
                    { 2, "Spiralli, dikişli, kareli ve çizgili defterler", "Defterler" },
                    { 3, "Kuru, sulu, pastel boyalar", "Boya Kalemleri" },
                    { 4, "LGS, YKS, KPSS hazırlık ve soru bankaları", "Test Kitapları" },
                    { 5, "Zımba, bant, makas, ataş", "Ofis & Masaüstü" },
                    { 6, "Sınav silgileri, mekanik kalemtıraşlar", "Silgi & Kalemtıraş" },
                    { 7, "Fotokopi kağıtları, notluklar", "Kağıt Ürünleri" },
                    { 8, "Okul çantaları ve beslenme kapları", "Çanta & Beslenme" },
                    { 9, "Klasörler, poşet dosyalar", "Dosyalama" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BrandId", "CategoryId", "Description", "ImageUrl", "Name", "Price", "StockQuantity" },
                values: new object[,]
                {
                    { 1, 1, 1, "Versatil Kalem", "https://placehold.co/400x400?text=Faber+Grip+2011", "Faber-Castell Grip 2011 0.7mm", 250m, 100 },
                    { 2, 1, 1, "Uçlu Kalem", "https://placehold.co/400x400?text=Faber+Tri+Click", "Faber-Castell Tri-Click 0.5mm", 45m, 200 },
                    { 3, 2, 1, "Mekanik Kalem", "https://placehold.co/400x400?text=Rotring+Tikky", "Rotring Tikky 0.7mm Bordo", 185m, 150 },
                    { 4, 2, 1, "Profesyonel Çizim Kalemi", "https://placehold.co/400x400?text=Rotring+500", "Rotring 500 0.5mm Siyah", 450m, 50 },
                    { 5, 3, 1, "Fosforlu Kalem", "https://placehold.co/400x400?text=Stabilo+Boss", "Stabilo Boss Original Sarı", 55m, 300 },
                    { 6, 3, 1, "İnce Uçlu Keçeli", "https://placehold.co/400x400?text=Stabilo+Point+88", "Stabilo Point 88 10'lu Set", 320m, 80 },
                    { 7, 4, 1, "Sınav Kalemi", "https://placehold.co/400x400?text=Adel+Kursun+Kalem", "Adel Blackline Kurşun Kalem", 15m, 500 },
                    { 8, 4, 1, "Öğrenci Uçlu Kalem", "https://placehold.co/400x400?text=Adel+Versatil", "Adel 0.7mm Versatil", 35m, 250 },
                    { 9, 5, 1, "Klasik Tükenmez", "https://placehold.co/400x400?text=Bic+Cristal", "Bic Cristal Mavi Tükenmez 5'li", 45m, 400 },
                    { 10, 5, 1, "Çoklu Tükenmez", "https://placehold.co/400x400?text=Bic+4+Renk", "Bic 4 Renk Tükenmez Kalem", 85m, 120 },
                    { 11, 1, 2, "Spiralli Defter", "https://placehold.co/400x400?text=Faber+A4+Defter", "Faber-Castell A4 Çizgili 96 Yp", 85m, 200 },
                    { 12, 1, 2, "Sert Kapak", "https://placehold.co/400x400?text=Faber+A5+Defter", "Faber-Castell A5 Kareli 80 Yp", 65m, 150 },
                    { 13, 6, 2, "Seperatörlü Defter", "https://placehold.co/400x400?text=Gipta+A4+Defter", "Gıpta A4 120 Yaprak 5 Bölmeli", 185m, 100 },
                    { 14, 6, 2, "Çizgisiz Not Defteri", "https://placehold.co/400x400?text=Gipta+Notluk", "Gıpta A5 Pastel Notluk", 55m, 300 },
                    { 15, 4, 2, "Kılavuz Çizgili", "https://placehold.co/400x400?text=Adel+Yazi+Defteri", "Adel İlkokul Güzel Yazı Defteri", 25m, 400 },
                    { 16, 7, 2, "Deri Kapaklı Ajanda", "https://placehold.co/400x400?text=Moleskine+Classic", "Moleskine Classic Pocket Siyah", 850m, 30 },
                    { 17, 7, 2, "İnce Not Defteri", "https://placehold.co/400x400?text=Moleskine+Cahier", "Moleskine Cahier Journal 3'lü", 450m, 45 },
                    { 18, 8, 2, "120gr Kağıt", "https://placehold.co/400x400?text=Keskin+Color+Resim", "Keskin Color A4 Spiralli Resim Defteri", 95m, 180 },
                    { 19, 8, 2, "Dikişli", "https://placehold.co/400x400?text=Keskin+Color+Muzik", "Keskin Color Müzik Defteri", 35m, 200 },
                    { 20, 1, 3, "Karton Kutu", "https://placehold.co/400x400?text=Faber+24lu+Kuru+Boya", "Faber-Castell 24'lü Kuru Boya", 275m, 120 },
                    { 21, 1, 3, "Fırça Hediyeli", "https://placehold.co/400x400?text=Faber+12li+Sulu+Boya", "Faber-Castell 12'li Sulu Boya", 145m, 150 },
                    { 22, 4, 3, "Çanta Boy", "https://placehold.co/400x400?text=Adel+Pastel+Boya", "Adel 12'li Yağlı Pastel Boya", 85m, 200 },
                    { 23, 4, 3, "Yarım Boy", "https://placehold.co/400x400?text=Adel+Kuru+Boya", "Adel 12'li Kuru Boya", 65m, 250 },
                    { 24, 3, 3, "Bebek/Çocuk Boyası", "https://placehold.co/400x400?text=Stabilo+Woody", "Stabilo Woody 3 in 1 10'lu", 650m, 40 },
                    { 25, 9, 3, "Yıkanabilir", "https://placehold.co/400x400?text=Carioca+Joy", "Carioca Joy 12'li Keçeli Kalem", 125m, 180 },
                    { 26, 9, 3, "Kalın Uçlu", "https://placehold.co/400x400?text=Carioca+Jumbo", "Carioca Jumbo 6'lı Keçeli", 110m, 90 },
                    { 27, 10, 4, "Yeni Müfredat", "https://placehold.co/400x400?text=Limit+TYT+Turkce", "Limit TYT Türkçe Soru Bankası", 240m, 300 },
                    { 28, 10, 4, "Çözümlü", "https://placehold.co/400x400?text=Limit+AYT+Edebiyat", "Limit AYT Edebiyat Soru Bankası", 260m, 250 },
                    { 29, 11, 4, "Video Çözümlü", "https://placehold.co/400x400?text=Sarmal+TYT+Mat", "Bilgi Sarmal TYT Matematik", 295m, 400 },
                    { 30, 11, 4, "Soru Bankası", "https://placehold.co/400x400?text=Sarmal+11+Fizik", "Bilgi Sarmal 11. Sınıf Fizik", 220m, 150 },
                    { 31, 12, 4, "Zor Seviye", "https://placehold.co/400x400?text=3D+TYT+Matematik", "3D TYT Matematik Simülasyon", 310m, 350 },
                    { 32, 12, 4, "Tamamı Çözümlü", "https://placehold.co/400x400?text=3D+AYT+Geometri", "3D AYT Geometri", 280m, 200 },
                    { 33, 13, 4, "Set Halinde", "https://placehold.co/400x400?text=Cap+TYT+Fizik", "Çap TYT Fizik Fasikülleri", 350m, 120 },
                    { 34, 14, 4, "Hızlandırılmış", "https://placehold.co/400x400?text=Hiz+LGS+Paragraf", "Hız Yayınları 8. Sınıf LGS Paragraf", 180m, 500 },
                    { 35, 14, 4, "Yeni Nesil", "https://placehold.co/400x400?text=Hiz+LGS+Matematik", "Hız Yayınları 8. Sınıf LGS Matematik", 195m, 450 },
                    { 36, 15, 4, "Klasikleşmiş", "https://placehold.co/400x400?text=Palme+TYT+Kimya", "Palme TYT Kimya Soru Bankası", 250m, 220 },
                    { 37, 15, 4, "Konu Anlatımlı", "https://placehold.co/400x400?text=Palme+10+Biyoloji", "Palme 10. Sınıf Biyoloji", 270m, 140 },
                    { 38, 16, 4, "LGS Hazırlık", "https://placehold.co/400x400?text=Tonguc+8+Matematik", "Tonguç 8. Sınıf Dinamo Matematik", 210m, 600 },
                    { 39, 16, 4, "Taktikli Soru Bankası", "https://placehold.co/400x400?text=Tonguc+Paragrafiks", "Tonguç TYT Paragrafiks", 175m, 450 },
                    { 40, 17, 5, "Metal Fileli", "https://placehold.co/400x400?text=MAS+Organizer", "MAS Masaüstü Organizer Set Siyah", 240m, 80 },
                    { 41, 17, 5, "Yedek Tel", "https://placehold.co/400x400?text=MAS+Zimba+Teli", "MAS No:10 Zımba Teli 1000'li", 25m, 800 },
                    { 42, 18, 5, "Paslanmaz Çelik", "https://placehold.co/400x400?text=Maped+Makas", "Maped 21cm Ofis Makası", 75m, 150 },
                    { 43, 18, 5, "20 Sayfa Kapasite", "https://placehold.co/400x400?text=Maped+Delgec", "Maped Ergonomik Delgeç", 165m, 90 },
                    { 44, 19, 5, "Büyük Boy", "https://placehold.co/400x400?text=Pritt+Stick", "Pritt Stick Yapıştırıcı 43g", 65m, 300 },
                    { 45, 19, 5, "Çok Amaçlı", "https://placehold.co/400x400?text=Pritt+Sivi", "Pritt Sıvı Yapıştırıcı", 55m, 250 },
                    { 46, 1, 5, "İz Bırakmaz", "https://placehold.co/400x400?text=Faber+Tack-It", "Faber-Castell Tack-It Hamur Yapıştırıcı", 45m, 400 },
                    { 47, 5, 5, "Silinebilir", "https://placehold.co/400x400?text=Bic+Tahta+Kalemi", "Bic Beyaz Tahta Kalemi 4'lü", 120m, 180 },
                    { 48, 1, 6, "Toz Bırakmaz", "https://placehold.co/400x400?text=Faber+Dust-Free", "Faber-Castell Dust-Free Sınav Silgisi", 25m, 1000 },
                    { 49, 1, 6, "Haznesiz", "https://placehold.co/400x400?text=Faber+Metal+Kalemtiras", "Faber-Castell Çift Delikli Metal Kalemtıraş", 45m, 300 },
                    { 50, 2, 6, "Yumuşak Silgi", "https://placehold.co/400x400?text=Rotring+Silgi", "Rotring Tikky Silgi 2'li", 35m, 400 },
                    { 51, 3, 6, "Renkli", "https://placehold.co/400x400?text=Stabilo+Silgi", "Stabilo Legacy Silgi", 20m, 500 },
                    { 52, 18, 6, "Dökülmez", "https://placehold.co/400x400?text=Maped+Shaker", "Maped Shaker Hazneli Kalemtıraş", 58m, 200 },
                    { 53, 18, 6, "Korumalı", "https://placehold.co/400x400?text=Maped+Salyangoz", "Maped Salyangoz Silgi", 35m, 150 },
                    { 54, 20, 6, "İspanyol Yapımı", "https://placehold.co/400x400?text=Milan+430+Silgi", "Milan 430 Klasik Kauçuk Silgi", 15m, 600 },
                    { 55, 20, 6, "2'si 1 Arada", "https://placehold.co/400x400?text=Milan+Capsule", "Milan Capsule Silgili Kalemtıraş", 85m, 250 },
                    { 56, 21, 7, "500'lü Paket", "https://placehold.co/400x400?text=Navigator+A4", "Navigator A4 80g Fotokopi Kağıdı", 165m, 500 },
                    { 57, 6, 7, "10 Renk", "https://placehold.co/400x400?text=Gipta+Elisi+Kagidi", "Gıpta Renkli A4 Elişi Kağıdı", 45m, 300 },
                    { 58, 6, 7, "Su Geçirmez", "https://placehold.co/400x400?text=Gipta+Yapiskanli+Notluk", "Gıpta Şeffaf Yapışkanlı Notluk", 35m, 400 },
                    { 59, 22, 7, "Koli Bazlı Fotokopi Kağıdı", "https://placehold.co/400x400?text=Mopak+A4+Kutu", "Mopak A4 80g Kutu (5 Paket)", 800m, 50 },
                    { 60, 22, 7, "20'li Paket", "https://placehold.co/400x400?text=Mopak+Resim+Kagidi", "Mopak Resim Kağıdı 25x35", 40m, 200 },
                    { 61, 23, 7, "75x75 Yapışkanlı", "https://placehold.co/400x400?text=Ve-Ge+Kup+Not", "Ve-Ge Sarı Küp Not", 25m, 600 },
                    { 62, 23, 7, "Çizim İçin", "https://placehold.co/400x400?text=Ve-Ge+Aydinger", "Ve-Ge Aydınger Kağıdı A4", 120m, 100 },
                    { 63, 24, 8, "Klasik Sırt Çantası", "https://placehold.co/400x400?text=Eastpak+Padded", "Eastpak Padded Pak'r Siyah", 1850m, 40 },
                    { 64, 24, 8, "Tek Bölmeli", "https://placehold.co/400x400?text=Eastpak+Benchmark", "Eastpak Benchmark Kalemlik", 450m, 60 },
                    { 65, 4, 8, "Sırt Destekli", "https://placehold.co/400x400?text=Adel+Canta", "Adel Ortopedik İlkokul Çantası", 750m, 50 },
                    { 66, 4, 8, "Kilitli", "https://placehold.co/400x400?text=Adel+Beslenme+Kabi", "Adel 3 Bölmeli Çelik Beslenme Kabı", 320m, 80 },
                    { 67, 18, 8, "BPA İçermez", "https://placehold.co/400x400?text=Maped+Suluk", "Maped Picnik Concept Suluk 430ml", 295m, 100 },
                    { 68, 18, 8, "Mikrodalgaya Girebilir", "https://placehold.co/400x400?text=Maped+Beslenme+Kutusu", "Maped Çift Katlı Beslenme Kutusu", 350m, 75 },
                    { 69, 25, 9, "Çelik Mekanizmalı", "https://placehold.co/400x400?text=Noki+Klasor", "Noki A4 Geniş Klasör Mavi", 85m, 300 },
                    { 70, 25, 9, "A4 Uyumlu Föy", "https://placehold.co/400x400?text=Noki+Poset+Dosya", "Noki 100'lü Şeffaf Poşet Dosya", 95m, 500 },
                    { 71, 17, 9, "Şeffaf Evrak Taşıma", "https://placehold.co/400x400?text=MAS+Citcitli+Dosya", "MAS Çıtçıtlı Zarf Dosya 10'lu", 75m, 400 },
                    { 72, 17, 9, "Plastik", "https://placehold.co/400x400?text=MAS+Telli+Dosya", "MAS Telli Dosya 50'li Paket", 150m, 200 },
                    { 73, 6, 9, "Sabit Föylü", "https://placehold.co/400x400?text=Gipta+Sunum+Dosyasi", "Gıpta Sunum Dosyası 40 Yaprak", 95m, 150 },
                    { 74, 6, 9, "13 Bölmeli", "https://placehold.co/400x400?text=Gipta+Evrak+Cantasi", "Gıpta Körüklü Evrak Çantası", 220m, 90 },
                    { 75, 26, 9, "Çekmece İçi", "https://placehold.co/400x400?text=Esselte+Askili+Dosya", "Esselte Askılı Dosya 25'li", 450m, 40 },
                    { 76, 26, 9, "Karton Kapak", "https://placehold.co/400x400?text=Esselte+Dar+Klasor", "Esselte Dar Klasör Siyah", 80m, 120 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Name",
                table: "Brands",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId_ProductId",
                table: "CartItems",
                columns: new[] { "CartId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
