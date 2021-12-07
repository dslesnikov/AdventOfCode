extern crate proc_macro;

use proc_macro::TokenStream;
use quote::quote;
use syn;

#[proc_macro_derive(CreateFromLines)]
pub fn create_line_macro_derive(input: TokenStream) -> TokenStream {
    let ast = syn::parse(input).unwrap();

    impl_lines_create_macro(&ast)
}

#[proc_macro_derive(CreateFromContent)]
pub fn create_content_macro_derive(input: TokenStream) -> TokenStream {
    let ast = syn::parse(input).unwrap();

    impl_content_create_macro(&ast)
}

fn impl_lines_create_macro(ast: &syn::DeriveInput) -> TokenStream {
    let name = &ast.ident;
    let gen = quote! {
        impl crate::parsing::CreateFromLines for #name {
            fn create() -> Self {
                let parser = crate::parsing::InputParser;
                parser.from_lines::<#name>()
            }
        }
    };
    gen.into()
}

fn impl_content_create_macro(ast: &syn::DeriveInput) -> TokenStream {
    let name = &ast.ident;
    let gen = quote! {
        impl crate::parsing::CreateFromContent for #name {
            fn create() -> Self {
                let parser = crate::parsing::InputParser;
                parser.from_content::<#name>()
            }
        }
    };
    gen.into()
}
