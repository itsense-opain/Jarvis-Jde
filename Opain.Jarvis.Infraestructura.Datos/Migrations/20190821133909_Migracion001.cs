using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Opain.Jarvis.Infraestructura.Datos.Migrations
{
    public partial class Migracion001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accesos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Grupo = table.Column<string>(maxLength: 45, nullable: false),
                    Rol = table.Column<string>(maxLength: 45, nullable: false),
                    NombreUsuario = table.Column<string>(maxLength: 150, nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Hora = table.Column<string>(maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accesos", x => x.Id);
                });

            migrationBuilder.CreateTable(
               name: "PoliticasDeTratamientoDeDatos",
               columns: table => new
               {
                   Id = table.Column<int>(nullable: false).Annotation("MySQL:AutoIncrement", true),
                   NombreUsuario = table.Column<string>(maxLength: 150, nullable: false),
                   AceptarPoliticas = table.Column<int>(nullable: false), //table.Column<bool>(nullable: false),
                   Fecha = table.Column<DateTime>(nullable: false),
                   Hora = table.Column<string>(maxLength: 5, nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_PoliticasDeTratamientoDeDatos", x => x.Id);
               });

            migrationBuilder.CreateTable(
                name: "Aerolineas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Nombre = table.Column<string>(maxLength: 150, nullable: false),
                    IdEstado = table.Column<int>(nullable: false),
                    PDFPasajeros = table.Column<int>(nullable: false),
                    CantidadUsuarios = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aerolineas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CargueArchivos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Aerolinea = table.Column<string>(maxLength: 45, nullable: false),
                    TipoArchivo = table.Column<string>(maxLength: 45, nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargueArchivos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cargues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    FechaHora = table.Column<DateTime>(nullable: false),
                    Usuario = table.Column<string>(maxLength: 50, nullable: false),
                    Tipo = table.Column<int>(nullable: false),
                    Archivo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Causales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Codigo = table.Column<string>(maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(maxLength: 250, nullable: false),
                    Tipo = table.Column<int>(nullable: false),
                    Estado = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Causales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorariosOperaciones",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Dia = table.Column<string>(maxLength: 1, nullable: false),
                    HoraInicio = table.Column<string>(maxLength: 5, nullable: false),
                    HoraFin = table.Column<string>(maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosOperaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Paises",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 2, nullable: false),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false),
                    IdEstado = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 150, nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TasasAeroportuarias",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    ValorCOP = table.Column<decimal>(nullable: false),
                    ValorUSD = table.Column<decimal>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TasasAeroportuarias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 150, nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<int>(type: "int", nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<int>(type: "int", nullable: false),
                    TwoFactorEnabled = table.Column<int>(type: "int", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<int>(type: "int", nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(nullable: true),
                    Apellido = table.Column<string>(nullable: true),
                    Cargo = table.Column<string>(nullable: true),
                    Telefono = table.Column<string>(nullable: true),
                    TipoDocumento = table.Column<string>(nullable: true),
                    NumeroDocumento = table.Column<string>(nullable: true),
                    Activo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorariosAerolineas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    IdAerolinea = table.Column<int>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    HoraInicio = table.Column<string>(maxLength: 5, nullable: false),
                    HoraFin = table.Column<string>(maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosAerolineas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorariosAerolineas_Aerolineas_IdAerolinea",
                        column: x => x.IdAerolinea,
                        principalTable: "Aerolineas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperacionesVuelos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    IdVuelo = table.Column<int>(nullable: false),
                    IdAerolinea = table.Column<int>(nullable: false),
                    TipoVuelo = table.Column<string>(nullable: false),
                    NumeroVuelo = table.Column<string>(nullable: false),
                    Destino = table.Column<string>(nullable: false),
                    FechaVuelo = table.Column<DateTime>(nullable: false),
                    MatriculaVuelo = table.Column<string>(maxLength: 15, nullable: false),
                    HoraVuelo = table.Column<string>(maxLength: 5, nullable: false),
                    TotalEmbarcados = table.Column<int>(nullable: false),
                    ConfirmacionPasajeros = table.Column<int>(nullable: false),
                    ConfirmacionTransitos = table.Column<int>(nullable: false),
                    ConfirmacionGenDec = table.Column<int>(nullable: false),
                    CanfirmacionManifiesto = table.Column<int>(nullable: false),
                    ConfirmacionOperacion = table.Column<int>(nullable: false),
                    INF = table.Column<int>(nullable: false),
                    TTL = table.Column<int>(nullable: false),
                    TTC = table.Column<int>(nullable: false),
                    EX = table.Column<int>(nullable: false),
                    TRIP = table.Column<int>(nullable: false),
                    PAX = table.Column<int>(nullable: false),
                    PagoCOP = table.Column<int>(nullable: false),
                    PagoUSD = table.Column<int>(nullable: false),
                    EstadoProceso = table.Column<int>(nullable: false),
                    IdCargue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperacionesVuelos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperacionesVuelos_Aerolineas_IdAerolinea",
                        column: x => x.IdAerolinea,
                        principalTable: "Aerolineas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperacionesVuelos_Cargues_IdCargue",
                        column: x => x.IdCargue,
                        principalTable: "Cargues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ciudades",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 3, nullable: false),
                    Nombre = table.Column<string>(maxLength: 150, nullable: false),
                    IdPais = table.Column<string>(nullable: false),
                    IdEstado = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ciudades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ciudades_Paises_IdPais",
                        column: x => x.IdPais,
                        principalTable: "Paises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimsRoles",
                columns: table => new
                {
                    Id = table.Column<int>(maxLength: 150, nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimsRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimsRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimsUsuario",
                columns: table => new
                {
                    Id = table.Column<int>(maxLength: 150, nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimsUsuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimsUsuario_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoginsUsuario",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 150, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 150, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginsUsuario", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_LoginsUsuario_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolesUsuarios",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 150, nullable: false),
                    RoleId = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesUsuarios", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_RolesUsuarios_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolesUsuarios_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    NumeroTicket = table.Column<string>(nullable: false),
                    TipoConsulta = table.Column<int>(nullable: false),
                    Asunto = table.Column<string>(nullable: false),
                    FechaVuelo = table.Column<DateTime>(nullable: false),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    Mensaje = table.Column<string>(nullable: false),
                    Adjunto = table.Column<string>(nullable: true),
                    Estado = table.Column<int>(nullable: false),
                    IdAerolinea = table.Column<int>(nullable: false),
                    IdUsuario = table.Column<string>(nullable: false),
                    Seguimiento = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Aerolineas_IdAerolinea",
                        column: x => x.IdAerolinea,
                        principalTable: "Aerolineas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TokensUsuario",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 150, nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 150, nullable: false),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokensUsuario", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_TokensUsuario_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosAerolineas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    IdAerolinea = table.Column<int>(nullable: false),
                    IdUsuario = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosAerolineas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuariosAerolineas_Aerolineas_IdAerolinea",
                        column: x => x.IdAerolinea,
                        principalTable: "Aerolineas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosAerolineas_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Archivos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Nombre = table.Column<string>(maxLength: 40, nullable: false),
                    IdOperacionVuelo = table.Column<int>(nullable: false),
                    Tipo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Archivos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Archivos_OperacionesVuelos_IdOperacionVuelo",
                        column: x => x.IdOperacionVuelo,
                        principalTable: "OperacionesVuelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NovedadesProcesos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    IdOperacionVuelo = table.Column<int>(nullable: false),
                    TipoNovedad = table.Column<int>(nullable: false),
                    IdCausal = table.Column<int>(nullable: false),
                    Descripcion = table.Column<string>(nullable: false),
                    FechaHora = table.Column<DateTime>(nullable: false),
                    IdRegistro = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NovedadesProcesos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NovedadesProcesos_Causales_IdCausal",
                        column: x => x.IdCausal,
                        principalTable: "Causales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NovedadesProcesos_OperacionesVuelos_IdOperacionVuelo",
                        column: x => x.IdOperacionVuelo,
                        principalTable: "OperacionesVuelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pasajeros",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    IdOperacionVuelo = table.Column<int>(nullable: false),
                    NombrePasajero = table.Column<string>(maxLength: 250, nullable: false),
                    IdCategoriaPasajero = table.Column<string>(nullable: false),
                    FechaVuelo = table.Column<DateTime>(nullable: false),
                    NumeroVuelo = table.Column<string>(nullable: false),
                    MatriculaVuelo = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pasajeros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pasajeros_OperacionesVuelos_IdOperacionVuelo",
                        column: x => x.IdOperacionVuelo,
                        principalTable: "OperacionesVuelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PasajerosTransito",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    IdOperacionVuelo = table.Column<int>(nullable: false),
                    FechaLlegada = table.Column<DateTime>(nullable: false),
                    HoraLlegada = table.Column<string>(nullable: false),
                    IdVueloLlegada = table.Column<int>(nullable: false),
                    NumeroVueloLlegada = table.Column<string>(nullable: false),
                    Origen = table.Column<string>(nullable: false),
                    FechaSalida = table.Column<DateTime>(nullable: false),
                    HoraSalida = table.Column<string>(nullable: false),
                    IdVueloSalida = table.Column<int>(nullable: false),
                    NumeroVueloSalida = table.Column<string>(nullable: false),
                    Destino = table.Column<string>(nullable: false),
                    NombrePasajero = table.Column<string>(maxLength: 250, nullable: false),
                    Categoria = table.Column<string>(maxLength: 5, nullable: false),
                    FechaHoraCargue = table.Column<DateTime>(nullable: false),
                    FechaHoraFirma = table.Column<DateTime>(nullable: false),
                    Firmado = table.Column<int>(nullable: false),
                    AerolineaLlegada = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasajerosTransito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasajerosTransito_OperacionesVuelos_IdOperacionVuelo",
                        column: x => x.IdOperacionVuelo,
                        principalTable: "OperacionesVuelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vuelos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    NumeroVuelo = table.Column<string>(maxLength: 10, nullable: false),
                    IdAerolinea = table.Column<int>(nullable: false),
                    IdOrigen = table.Column<string>(nullable: false),
                    IdDestino = table.Column<string>(nullable: false),
                    IdEstado = table.Column<int>(nullable: false),
                    TipoVuelo = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vuelos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vuelos_Aerolineas_IdAerolinea",
                        column: x => x.IdAerolinea,
                        principalTable: "Aerolineas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vuelos_Ciudades_IdDestino",
                        column: x => x.IdDestino,
                        principalTable: "Ciudades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vuelos_Ciudades_IdOrigen",
                        column: x => x.IdOrigen,
                        principalTable: "Ciudades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RespuestasTickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    IdTicket = table.Column<int>(nullable: false),
                    Mensaje = table.Column<string>(nullable: false),
                    Adjunto = table.Column<string>(nullable: true),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    IdUsuario = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RespuestasTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RespuestasTickets_Tickets_IdTicket",
                        column: x => x.IdTicket,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RespuestasTickets_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperacionesVuelosMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    IdVuelo = table.Column<int>(nullable: false),
                    FechaVuelo = table.Column<DateTime>(nullable: false),
                    HoraVuelo = table.Column<string>(maxLength: 5, nullable: false),
                    MatriculaVuelo = table.Column<string>(maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperacionesVuelosMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperacionesVuelosMaster_Vuelos_IdVuelo",
                        column: x => x.IdVuelo,
                        principalTable: "Vuelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Archivos_IdOperacionVuelo",
                table: "Archivos",
                column: "IdOperacionVuelo");

            migrationBuilder.CreateIndex(
                name: "IX_Ciudades_IdPais",
                table: "Ciudades",
                column: "IdPais");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimsRoles_RoleId",
                table: "ClaimsRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimsUsuario_UserId",
                table: "ClaimsUsuario",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HorariosAerolineas_IdAerolinea",
                table: "HorariosAerolineas",
                column: "IdAerolinea");

            migrationBuilder.CreateIndex(
                name: "IX_LoginsUsuario_UserId",
                table: "LoginsUsuario",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NovedadesProcesos_IdCausal",
                table: "NovedadesProcesos",
                column: "IdCausal");

            migrationBuilder.CreateIndex(
                name: "IX_NovedadesProcesos_IdOperacionVuelo",
                table: "NovedadesProcesos",
                column: "IdOperacionVuelo");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionesVuelos_IdAerolinea",
                table: "OperacionesVuelos",
                column: "IdAerolinea");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionesVuelos_IdCargue",
                table: "OperacionesVuelos",
                column: "IdCargue");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionesVuelosMaster_IdVuelo",
                table: "OperacionesVuelosMaster",
                column: "IdVuelo");

            migrationBuilder.CreateIndex(
                name: "IX_Pasajeros_IdOperacionVuelo",
                table: "Pasajeros",
                column: "IdOperacionVuelo");

            migrationBuilder.CreateIndex(
                name: "IX_PasajerosTransito_IdOperacionVuelo",
                table: "PasajerosTransito",
                column: "IdOperacionVuelo");

            migrationBuilder.CreateIndex(
                name: "IX_RespuestasTickets_IdTicket",
                table: "RespuestasTickets",
                column: "IdTicket");

            migrationBuilder.CreateIndex(
                name: "IX_RespuestasTickets_IdUsuario",
                table: "RespuestasTickets",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolesUsuarios_RoleId",
                table: "RolesUsuarios",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_IdAerolinea",
                table: "Tickets",
                column: "IdAerolinea");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_IdUsuario",
                table: "Tickets",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Usuario",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Usuario",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosAerolineas_IdAerolinea",
                table: "UsuariosAerolineas",
                column: "IdAerolinea");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosAerolineas_IdUsuario",
                table: "UsuariosAerolineas",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Vuelos_IdAerolinea",
                table: "Vuelos",
                column: "IdAerolinea");

            migrationBuilder.CreateIndex(
                name: "IX_Vuelos_IdDestino",
                table: "Vuelos",
                column: "IdDestino");

            migrationBuilder.CreateIndex(
                name: "IX_Vuelos_IdOrigen",
                table: "Vuelos",
                column: "IdOrigen");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accesos");

            migrationBuilder.DropTable(
                name: "Archivos");

            migrationBuilder.DropTable(
                name: "CargueArchivos");

            migrationBuilder.DropTable(
                name: "ClaimsRoles");

            migrationBuilder.DropTable(
                name: "ClaimsUsuario");

            migrationBuilder.DropTable(
                name: "HorariosAerolineas");

            migrationBuilder.DropTable(
                name: "HorariosOperaciones");

            migrationBuilder.DropTable(
                name: "LoginsUsuario");

            migrationBuilder.DropTable(
                name: "NovedadesProcesos");

            migrationBuilder.DropTable(
                name: "OperacionesVuelosMaster");

            migrationBuilder.DropTable(
                name: "Pasajeros");

            migrationBuilder.DropTable(
                name: "PasajerosTransito");

            migrationBuilder.DropTable(
                name: "RespuestasTickets");

            migrationBuilder.DropTable(
                name: "RolesUsuarios");

            migrationBuilder.DropTable(
                name: "TasasAeroportuarias");

            migrationBuilder.DropTable(
                name: "TokensUsuario");

            migrationBuilder.DropTable(
                name: "UsuariosAerolineas");

            migrationBuilder.DropTable(
                name: "Causales");

            migrationBuilder.DropTable(
                name: "Vuelos");

            migrationBuilder.DropTable(
                name: "OperacionesVuelos");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Ciudades");

            migrationBuilder.DropTable(
                name: "Cargues");

            migrationBuilder.DropTable(
                name: "Aerolineas");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Paises");
        }
    }
}
