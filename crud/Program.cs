using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace crud
{
    enum Opcoes
    {

        EXIT,
        CREAD,
        READ,
        UPDATE,
        DELETE,
        SELECT

    }

    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DatadeNascimento { get; set; }
        public string Telefone { get; set; }

        public Pessoa(int id, string nome, DateTime Datanascimento, string telefone)
        {

            Id = id;
            Nome = nome;
            DatadeNascimento = Datanascimento;
            Telefone = telefone;

        }
        public Pessoa(string nome, DateTime Datanascimento, string telefone)
        {

            Nome = nome;
            DatadeNascimento = Datanascimento;
            Telefone = telefone;

        }
    }
    class Program
    {
        static string connection = @"Data Source=DESKTOP-H20UE5F\SQLEXPRESS;Initial Catalog=crudsql;Integrated Security=True;";
        public List<Pessoa> pessoa = new List<Pessoa>();

        static void Main(string[] args)
        {

            int opcao = Convert.ToInt32(Console.ReadLine())
            

            while ((int)Opcoes.EXIT == 0)
            {
                

                if ((int)Opcoes.CREAD == 1)
                {

                    Console.WriteLine("Insira o Nome:");
                    string nome = Console.ReadLine();

                    Console.WriteLine("Insira o Telefone:");
                    string telefone = Console.ReadLine();

                    Console.WriteLine("Insira o Data de nascimento:");
                    DateTime dataDeNascimento = Convert.ToDateTime(Console.ReadLine());

                    Pessoa pessoa = new Pessoa(nome, dataDeNascimento, telefone);

                    Create(pessoa);
                }

                if((int)Opcoes.DELETE == 4) {

                    Console.WriteLine("Insira um termo para busca:");
                    string termoDelete = Console.ReadLine();
                    List<Pessoa> pessoasEncontradasDelete = EncontrarPessoa(termoDelete);

                    Pessoa pessoaEncontradaDelete = pessoasEncontradasDelete.First();

                    RemoverPessoa(pessoaEncontradaDelete);

                    

                if ((int)Opcoes.UPDATE == 3)
                {


                    Console.WriteLine("Insira um termo para busca:");
                    string termoUpdate = Console.ReadLine();
                    List<Pessoa> pessoasEncontradasUpdate = EncontrarPessoa(termoUpdate);

                    Pessoa pessoaEncontradaUpdate = pessoasEncontradasUpdate.First();

                    Console.WriteLine("Insira o Nome:");
                    string nome = Console.ReadLine();

                    Console.WriteLine("Insira o Telefone:");
                    string telefone = Console.ReadLine();

                    Console.WriteLine("Insira o Data de nascimento:");
                    DateTime dataDeNascimento = Convert.ToDateTime(Console.ReadLine());

                    AtualizarPessoa(pessoaEncontradaUpdate);


                }

            }
        }
        static void Create(Pessoa pessoa)
        {
            try
            {

                var query = @"INSERT INTO Pessoa (Nome, DatadeNascimento, Telefone)
                        values(@nome, @Datanascimento, @telefone)";

                using (var sql = new SqlConnection(connection))
                {

                    SqlCommand command = new SqlCommand(query, sql);
                    command.Parameters.AddWithValue("@nome", pessoa.Nome);
                    command.Parameters.AddWithValue("@Datanascimento", pessoa.DatadeNascimento);
                    command.Parameters.AddWithValue("@telefone", pessoa.Telefone);
                    command.Connection.Open();
                    command.ExecuteNonQuery();

                }

            
                Console.WriteLine("Pessoa cadastrada com sucesso.");

            }
            catch (Exception ex)
            {

                Console.WriteLine("Erro: " + ex.Message);
           
            }
        }

        static void AtualizarPessoa(Pessoa pessoa)
        {

            try
            {
                var query = @"UPDATE Pessoa
                            SET Nome = @nome,
                            DatadeNascimento = @Datanascimento,
                            Telefone = @telefone
                            WHERE Id = @id";

                using (var sql = new SqlConnection(connection))

                {

                    SqlCommand command = new SqlCommand(query, sql);
                    command.Parameters.AddWithValue("@nome", pessoa.Nome);
                    command.Parameters.AddWithValue("@Datanascimento", pessoa.DatadeNascimento);
                    command.Parameters.AddWithValue("@telefone", pessoa.Telefone);
                    command.Connection.Open();
                    command.ExecuteNonQuery();

                }
                Console.WriteLine("Pessoa atualizada com sucesso.");
            }
            catch (Exception ex)
            {

                Console.WriteLine("Erro: " + ex.Message);

            }

        }

        static List<Pessoa> EncontrarPessoa(string termo)
        {

            List<Pessoa> pessoas = new List<Pessoa>();
            SqlDataReader resultado;

            try
            {

                var query = @"SELECT Id, Nome, DatadeNascimento, Telefone
                            WHERE Nome like CONCAT ('%', @termo, '%') OR DatadeNascimento like CONCAT ('%', @termo, '%')
                            OR Telefone like CONCAT ('%', @termo, '%')";

                using (var sql = new SqlConnection(connection))

                {

                    SqlCommand command = new SqlCommand(query, sql);
                    command.Parameters.AddWithValue("@termo", termo);
                    command.Connection.Open();
                    resultado = command.ExecuteReader();

                    while (resultado.Read())
                    {
                        pessoas.Add(new Pessoa(resultado.GetInt32(resultado.GetOrdinal("Id")),
                            resultado.GetString(resultado.GetOrdinal("Nome")),
                            resultado.GetDateTime(resultado.GetOrdinal("Datanascimento")),
                            resultado.GetString(resultado.GetOrdinal("Telefone"))));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
            return pessoas;

        }

            static void RemoverPessoa(Pessoa pessoa)
            {

                try
                {
                    var query = @"Delete from Pessoa
                                  Where = @id";
                    using (var sql = new SqlConnection(connection))
                    {

                        SqlCommand command = new SqlCommand(query, sql);
                        command.Parameters.AddWithValue("@id", pessoa.Id);
                        command.Connection.Open();
                        command.ExecuteNonQuery();

                    }
                    Console.WriteLine("Pessoa removida com sucesso.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                }

            }

        }

        
    }


}
