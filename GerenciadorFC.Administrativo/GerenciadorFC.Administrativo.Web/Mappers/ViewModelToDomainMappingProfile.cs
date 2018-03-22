using AutoMapper;
using GerenciadorFC.Administrativo.Web.Models.Cadastro;
using GerenciadorFC.Administrativo.Web.Models.CadastroViewModels;
using GerenciadorFC.Administrativo.Web.Models.EnderecoDados;
using GerenciadorFC.Administrativo.Web.Models.PessoaDados;
using GerenciadorFC.Administrativo.Web.Models.RepresentanteLegal;

namespace GerenciadorFC.Administrativo.Web.Mappers
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
			Pessoa();
			Endereco();
			RepresentanteLegal();
		}
        public void Pessoa()
        {
            CreateMap<PessoaViewModels, Pessoa>()
				.ForMember(dest => dest.Codigo, opt => opt.MapFrom(scr => scr.CodigoPessoa));
		}
		public void Endereco()
		{
			CreateMap<PessoaViewModels, Endereco>()
				.ForMember(dest => dest.Codigo, opt => opt.MapFrom(scr => scr.CodigoEndereco));
			CreateMap<RepresentanteLegalViewModels, Endereco>()
				.ForMember(dest => dest.Codigo, opt => opt.MapFrom(scr => scr.CodigoEndereco));
		}
		private void RepresentanteLegal()
		{
			CreateMap< RepresentanteLegalViewModels, RepresentanteLegal>()
				.ForMember(dest => dest.Codigo, opt => opt.MapFrom(scr => scr.CodigoRepLegal))
				.ForMember(dest => dest.CodigoPessoa, opt => opt.MapFrom(scr => scr.CodigoPessoa));
		}
	}
}

