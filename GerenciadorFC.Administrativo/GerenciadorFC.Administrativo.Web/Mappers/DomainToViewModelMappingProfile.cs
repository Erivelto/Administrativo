using AutoMapper;
using GerenciadorFC.Administrativo.Web.Models.Cadastro;
using GerenciadorFC.Administrativo.Web.Models.EnderecoDados;
using GerenciadorFC.Administrativo.Web.Models.PessoaDados;

namespace GerenciadorFC.Administrativo.Web.Mappers
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
			Pessoa();
			Endereco();
		}
		private void Pessoa()
		{
			CreateMap<Pessoa, PessoaViewModels>();
		}
		private void Endereco()
		{
			CreateMap<Endereco, PessoaViewModels>();
				//.ForMember(dest => dest.CodigoEndereco , opt => opt.MapFrom( scr => scr.Codigo))
				//.ForMember(dest => dest.CodigoPessoa, opt => opt.MapFrom(scr => scr.CodigoPessoa))
				//.ForMember(dest => dest.CodigoRepLegal, opt => opt.MapFrom(scr => scr.CodigoRepLegal))
				//.ForMember(dest => dest.Bairro, opt => opt.MapFrom(scr => scr.Bairro))
				//.ForMember(dest => dest.CEP, opt => opt.MapFrom(scr => scr.CEP))
				//.ForMember(dest => dest.Cidade, opt => opt.MapFrom(scr => scr.Cidade))
				//.ForMember(dest => dest.CNAE, opt => opt.Ignore())
				//.ForMember(dest => dest.Codigo, opt => opt.Ignore())
				//.ForMember(dest => dest.Complemento, opt => opt.MapFrom(scr => scr.Complemento))
				//.ForMember(dest => dest.Contabilidade, opt => opt.Ignore())
				//.ForMember(dest => dest.DataAbertura, opt => opt.Ignore())
				//.ForMember(dest => dest.DataAtulizacao, opt => opt.Ignore())
				//.ForMember(dest => dest.DataInclusao, opt => opt.Ignore())
				//.ForMember(dest => dest.DescricaoAtividade, opt => opt.Ignore())
				//.ForMember(dest => dest.Documento, opt => opt.Ignore())
				//.ForMember(dest => dest.Excluido, opt => opt.Ignore())
				//.ForMember(dest => dest.ExcluidoEndereco, opt => opt.MapFrom(scr => scr.Excluido))
				//.ForMember(dest => dest.IncricaoMunicipal, opt => opt.Ignore())
				//.ForMember(dest => dest.Logradouro, opt => opt.MapFrom(scr => scr.Logradouro))
				//.ForMember(dest => dest.Nome, opt => opt.Ignore())
				//.ForMember(dest => dest.Numrero, opt => opt.MapFrom(scr => scr.Numrero))
				//.ForMember(dest => dest.Razao, opt => opt.Ignore())
				//.ForMember(dest => dest.Status, opt => opt.Ignore())
				//.ForMember(dest => dest.TipoEnd, opt => opt.MapFrom(scr => scr.TipoEnd))
				//.ForMember(dest => dest.TipoPessoa, opt => opt.Ignore())
				//.ForMember(dest => dest.UF, opt => opt.MapFrom(scr => scr.UF));
		}
	}
}


