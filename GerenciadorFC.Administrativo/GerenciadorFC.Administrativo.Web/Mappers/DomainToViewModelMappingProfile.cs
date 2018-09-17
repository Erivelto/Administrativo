using AutoMapper;
using GerenciadorFC.Administrativo.Web.Models.Cadastro;
using GerenciadorFC.Administrativo.Web.Models.CadastroViewModels;
using GerenciadorFC.Administrativo.Web.Models.EnderecoDados;
using GerenciadorFC.Administrativo.Web.Models.PessoaDados;
using GerenciadorFC.Administrativo.Web.Models.RepresentanteLegal;
using System;

namespace GerenciadorFC.Administrativo.Web.Mappers
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
			Pessoa();
			Endereco();
			RepresentanteLegal();
			PessoaUpload();
		}
		private void Pessoa()
		{
			CreateMap<Pessoa, PessoaViewModels>()
				.ForMember(dest => dest.CodigoPessoa, opt => opt.MapFrom(scr => scr.Codigo));
		}
		private void Endereco()
		{
			CreateMap<Endereco, PessoaViewModels>()
				.ForMember(dest => dest.CodigoEndereco, opt => opt.MapFrom(scr => scr.Codigo));
			CreateMap<Endereco, RepresentanteLegalViewModels>()
				.ForMember(dest => dest.CodigoEndereco, opt => opt.MapFrom(scr => scr.Codigo));
		}
		private void RepresentanteLegal()
		{
			CreateMap<RepresentanteLegal,RepresentanteLegalViewModels>()
				.ForMember(dest => dest.CodigoRepLegal, opt => opt.MapFrom(scr => scr.Codigo))
				.ForMember(dest => dest.CodigoPessoa, opt => opt.MapFrom(scr => scr.CodigoPessoa));
		}
		private void PessoaUpload()
		{
			CreateMap<PessoaUpload, PessoaUploadViewModels>()
				.ForMember(dest => dest.Codigo, opt => opt.MapFrom(scr => scr.Codigo))
				.ForMember(dest => dest.CodigoPessoa, opt => opt.MapFrom(scr => scr.CodigoPessoa))
				.ForMember(dest => dest.DataCriacao, opt => opt.MapFrom(scr => scr.DataCriacao))
				.ForMember(dest => dest.Arquivo, opt => opt.MapFrom(scr => scr.Arquivo))
				.ForMember(dest => dest.Tipo, opt => opt.MapFrom(scr => scr.Tipo))
				.ForMember(dest => dest.Excluido, opt => opt.MapFrom(scr => scr.Excluido));
		}
	}
}


