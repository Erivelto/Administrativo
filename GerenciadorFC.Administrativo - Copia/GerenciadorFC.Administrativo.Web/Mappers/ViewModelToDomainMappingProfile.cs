using AutoMapper;
using GerenciadorFC.Administrativo.Web.Models.Cadastro;
using GerenciadorFC.Administrativo.Web.Models.EnderecoDados;
using GerenciadorFC.Administrativo.Web.Models.PessoaDados;

namespace GerenciadorFC.Administrativo.Web.Mappers
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
			Pessoa();
			Endereco();
		}
        public void Pessoa()
        {
            CreateMap<PessoaViewModels, Pessoa>();
        }
		public void Endereco()
		{
			CreateMap<PessoaViewModels, Endereco>();
		}
	}
}

