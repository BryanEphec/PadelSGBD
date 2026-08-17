using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Padel.SGBD.Api.Model;
using Padel.SGBD.Api.Repositories.Interfaces;
using Padel.SGBD.Api.Services;
using Xunit;
using MatchModel = Padel.SGBD.Api.Model.Match;

namespace Padel.Sgbd.Tests
{
    public class ParticipationServiceTests
    {
        private readonly Mock<IParticipationRepository> _participationRepoMock = new();
        private readonly Mock<IMatchRepository> _matchRepoMock = new();
        private readonly Mock<IMembreRepository> _membreRepoMock = new();

        [Fact]
        public async Task RejoindreMatch_QuandMatchComplet_DoitLeverUneException()
        {
            // Arrange
            var matricule = "M010";
            var idMatch = 1;

            _matchRepoMock.Setup(r => r.GetMatchByIdAsync(idMatch))
                .ReturnsAsync(new MatchModel { IdMatch = idMatch });

            _membreRepoMock.Setup(r => r.GetByMatriculeAsync(matricule))
                .ReturnsAsync(new Membre { Matricule = matricule, SoldeDu = 0 });

            _participationRepoMock.Setup(r => r.CountByMatchIdAsync(idMatch))
                .ReturnsAsync(4);

            var service = new ParticipationService(
                _participationRepoMock.Object,
                _matchRepoMock.Object,
                _membreRepoMock.Object);

            // Act
            var action = async () => await service.RejoindreMatchAsync(matricule, idMatch);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*match est complet*");
        }

        [Fact]
        public async Task RejoindreMatch_JoueurDejaInscrit_DoitLeverUneException()
        {
            // Arrange
            var matricule = "M011";
            var idMatch = 2;

            _matchRepoMock.Setup(r => r.GetMatchByIdAsync(idMatch))
                .ReturnsAsync(new MatchModel { IdMatch = idMatch });

            _membreRepoMock.Setup(r => r.GetByMatriculeAsync(matricule))
                .ReturnsAsync(new Membre { Matricule = matricule, SoldeDu = 0 });

            _participationRepoMock.Setup(r => r.CountByMatchIdAsync(idMatch))
                .ReturnsAsync(2);

            _participationRepoMock.Setup(r => r.GetParticipationAsync(matricule, idMatch))
                .ReturnsAsync(new Participations { Matricule = matricule, IdMatch = idMatch });

            var service = new ParticipationService(
                _participationRepoMock.Object,
                _matchRepoMock.Object,
                _membreRepoMock.Object);

            // Act
            var action = async () => await service.RejoindreMatchAsync(matricule, idMatch);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*participe déjà*");
        }

        [Fact]
        public async Task EnregistrerPaiement_MontantInferieurA15Euros_DoitLeverUneException()
        {
            // Arrange
            var matricule = "M012";
            var idMatch = 3;

            _participationRepoMock.Setup(r => r.GetParticipationAsync(matricule, idMatch))
                .ReturnsAsync(new Participations { Matricule = matricule, IdMatch = idMatch });

            var service = new ParticipationService(
                _participationRepoMock.Object,
                _matchRepoMock.Object,
                _membreRepoMock.Object);

            // Act
            var action = async () => await service.EnregistrerPaiementAsync(matricule, idMatch, 10.00m);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*15.00 € minimum*");
        }
    }
}