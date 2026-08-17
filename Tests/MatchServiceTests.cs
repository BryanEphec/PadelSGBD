using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Padel.SGBD.Api.Dtos;
using Padel.SGBD.Api.Model;
using Padel.SGBD.Api.Repositories.Interfaces;
using Padel.SGBD.Api.Services;
using Xunit;

namespace Padel.Sgbd.Tests
{
    public class MatchServiceTests
    {
        private readonly Mock<IMatchRepository> _matchRepoMock = new();
        private readonly Mock<IMembreRepository> _membreRepoMock = new();

        [Fact]
        public async Task CreerReservation_MembreAvecSoldeDu_DoitLeverUneException()
        {
            // Arrange
            var matricule = "M001";
            var membreEndette = new Membre
            {
                Matricule = matricule,
                Nom = "Dupont",
                Prenom = "Jean",
                Type = "G",
                SoldeDu = 30.00m
            };

            _membreRepoMock.Setup(r => r.GetByMatriculeAsync(matricule))
                .ReturnsAsync(membreEndette);

            var service = new MatchService(_matchRepoMock.Object, _membreRepoMock.Object, null!);

            var dto = new MatchCreateDtos
            {
                IdTerrain = 1,
                DateHeure = DateTime.Now.AddDays(2),
                EstPrive = false
            };

            // Act
            var action = async () => await service.CreerReservationAsync(matricule, dto);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*solde impayé*");
        }

        [Fact]
        public async Task CreerReservation_MembreSousPenalite_DoitLeverUneException()
        {
            // Arrange
            var matricule = "M002";
            var membreSousPenalite = new Membre
            {
                Matricule = matricule,
                Nom = "Durand",
                Prenom = "Alice",
                Type = "G",
                SoldeDu = 0m,
                DateFinPenalite = DateTime.Now.AddDays(4)
            };

            _membreRepoMock.Setup(r => r.GetByMatriculeAsync(matricule))
                .ReturnsAsync(membreSousPenalite);

            var service = new MatchService(_matchRepoMock.Object, _membreRepoMock.Object, null!);

            var dto = new MatchCreateDtos
            {
                IdTerrain = 1,
                DateHeure = DateTime.Now.AddDays(2),
                EstPrive = false
            };

            // Act
            var action = async () => await service.CreerReservationAsync(matricule, dto);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*sous pénalité*");
        }

        [Fact]
        public async Task CreerReservation_MembreLibreDelaiDepasse_DoitLeverUneException()
        {
            // Arrange
            var matricule = "M003";
            var membreLibre = new Membre
            {
                Matricule = matricule,
                Nom = "Martin",
                Prenom = "Lucas",
                Type = "L",
                SoldeDu = 0m
            };

            _membreRepoMock.Setup(r => r.GetByMatriculeAsync(matricule))
                .ReturnsAsync(membreLibre);

            var service = new MatchService(_matchRepoMock.Object, _membreRepoMock.Object, null!);

            var dto = new MatchCreateDtos
            {
                IdTerrain = 1,
                DateHeure = DateTime.Now.AddDays(8),
                EstPrive = false
            };

            // Act
            var action = async () => await service.CreerReservationAsync(matricule, dto);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*ne vous autorise à réserver que jusqu'au*");
        }
    }
}