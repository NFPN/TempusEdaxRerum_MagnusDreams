using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagnusDrems.DAO
{
    public class ComandosSQL
    {
        Connection connection = new Connection();
        SqlCommand qLComand = new SqlCommand();
        public string mensagem = "";

        public SqlCommand defaulCommand(string nome, string vida = "3", string score = "0", string highScore = "0")
        {
            qLComand.CommandText = "insert into informaçãoPersonagem(vidaJogador,nomeJogador,score,highScore)" +
                "values(@vidas,@nomeJogador,@score,@highScore)";

            qLComand.Parameters.AddWithValue("@vidas", vida);
            qLComand.Parameters.AddWithValue("@nomeJogador", nome);
            qLComand.Parameters.AddWithValue("@score", score);
            qLComand.Parameters.AddWithValue("@highScore", highScore);

            return qLComand;
        }

        public void InsertData(string nome)
        {
            if(nome != null)
            {

          
            try
            {
                qLComand = defaulCommand(nome);
                qLComand.Connection = connection.connect();
                qLComand.ExecuteNonQuery();
                this.mensagem = "Jogador criado com sucesso!!";
            }
            catch (SqlException e)
            {
                this.mensagem = " Erro ao criar jogador \n" + e.Message;
            }
            connection.desconnect();
            }
        }

        public void InsertData(string nome, string vida, string score, string highScore) 
        {
            if (nome != null)
            {

                try
                {
                    qLComand = defaulCommand(nome);
                    qLComand = defaulCommand(vida);
                    qLComand = defaulCommand(score);
                    qLComand = defaulCommand(highScore);
                    qLComand.Connection = connection.connect();
                    qLComand.ExecuteNonQuery();
                    this.mensagem = "Fim de jogo cadatrado com sucesso!!";
                }
                catch (SqlException e)
                {
                    this.mensagem = " Erro ao criar cadatro \n" + e.Message;
                }
                connection.desconnect();
            }
        }

        public void UpdtadeDataName(string nome)//acho que precisa de todos os valores
        {
            qLComand.CommandText = "update into informaçãoPersonagem(vidaJogador,nomeJogador,score,highScore)" +
                "values(@vidas,@nomeJogador,@score,@highScore)";

            qLComand.Parameters.AddWithValue("@vidas", 3);
            qLComand.Parameters.AddWithValue("@nomeJogador", nome);
            qLComand.Parameters.AddWithValue("@score", 0);
            qLComand.Parameters.AddWithValue("@highScore", 0);

            try
            {
                qLComand.Connection = connection.connect();
                qLComand.ExecuteNonQuery();
                this.mensagem = "Jogador criado com sucesso!!";
            }
            catch (SqlException e)
            {
                this.mensagem = " Erro ao criar jogador \n" + e.Message;
            }
            connection.desconnect();
        }
        

    }

  
}
