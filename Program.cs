using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RedePerceptron
{
	public class Program
	{
		public static void Main(string[] args)
		{
			//inicializações
			int bias = -1;
			int quantidadeDeIteracoes = 0;
			PrintConsole print = new();

			//Criação das entradas com seus valores e saídas desejadas

			Entrada primeiraEntrada = AdicionarValoresEntrada(bias, 0.1, 0.4, 0.7, 1);
			Entrada segundaEntrada = AdicionarValoresEntrada(bias, 0.5, 0.7, 0.1, 1);
			Entrada terceiraEntrada = AdicionarValoresEntrada(bias, 0.6, 0.9, 0.8, -1);
			Entrada quartaEntrada = AdicionarValoresEntrada(bias, 0.3, 0.7, 0.2, -1);

			// Criação dos pesos iniciais
			Peso pesos = AdicionarValoresPeso();

			//Imprime os pesos iniciais
			PrintConsole.PrintPesosIniciais(pesos);

			// Lista de resultados inicializada como nula
			List<double>? resultado = new();


			// Loop de treinamento
			while (resultado is not null)
			{
				quantidadeDeIteracoes++;
				// Imprime os pesos atuais e a iteração
				PrintConsole.PrintPesosAtuais(pesos, quantidadeDeIteracoes);

				//Processa a primeira entrada
				resultado = ProcessarAmostra(pesos, primeiraEntrada);

				// Se o resultado não é nulo (erro), continua para a próxima entrada
				if (resultado is not null)
					continue;

				resultado = ProcessarAmostra(pesos, segundaEntrada);

				if (resultado is not null)
					continue;

				resultado = ProcessarAmostra(pesos, terceiraEntrada);

				if (resultado is not null)
					continue;

				resultado = ProcessarAmostra(pesos, quartaEntrada);
			}
			//Ao final, imprime os pesos finais e a quantidade de iterações
			PrintConsole.PrintPesosFinais(pesos);
			PrintConsole.PrintQuantidadeDeIteracoes(quantidadeDeIteracoes);
		}

		static Entrada AdicionarValoresEntrada(
			int bias,
			double segundoValor,
			double terceiroValor,
			double quartoValor,
			int valorDesejadoSaida)
		{
			//Cria uma nova instância da classe Entrada
			Entrada entrada = new();

			entrada.Valores.Add(bias);
			entrada.Valores.Add(segundoValor);
			entrada.Valores.Add(terceiroValor);
			entrada.Valores.Add(quartoValor);
			//// Define o valor desejado de saída na propriedade ValorDesejadoDeSaida
			entrada.ValorDesejadoDeSaida = valorDesejadoSaida;

			return entrada;
		}

		static Peso AdicionarValoresPeso()
		{
			Peso peso = new();

			// Estamos criando uma nova instância da classe Random, que será usada para gerar números aleatórios.
			Random random = new();

			double valorMinimo = -1;
			double valorMaximo = 1;
			//// Calcula e adiciona um valor aleatório à lista Valores da instância de Peso
			///// Estamos gerando um número aleatório entre valorMinimo e valorMaximo usando 
			///o método NextDouble() do objeto random
			peso.Valores.Add(random.NextDouble() * (valorMaximo - valorMinimo) + valorMinimo);
			peso.Valores.Add(random.NextDouble() * (valorMaximo - valorMinimo) + valorMinimo);
			peso.Valores.Add(random.NextDouble() * (valorMaximo - valorMinimo) + valorMinimo);
			peso.Valores.Add(random.NextDouble() * (valorMaximo - valorMinimo) + valorMinimo);

			return peso;
		}
		//Aqui, estamos declarando um método estático chamado ProcessarAmostra.
		//Ele recebe dois parâmetros: uma instância de Peso chamada pesos e uma instância de Entrada chamada entrada. Esse método processa a amostra e atualiza os pesos.
		static List<double>? ProcessarAmostra(Peso pesos, Entrada entrada)
		{
			Amostra amostra = new();
			//// Calcula a amostra usando o método CalcularAmostra da classe Amostra
			var resultadoAmostra = Amostra.CalcularAmostra(
				pesos.Valores,
				entrada.Valores,
				entrada.ValorDesejadoDeSaida);

			// Verifica se o resultado da amostra é nulo 
			if (resultadoAmostra is null)
				return null;
			// Atualiza os pesos usando o método AtualizarPesos da classe Peso
			pesos.Valores = Peso.AtualizarPesos(
				resultadoAmostra.Value.Item1,
				resultadoAmostra.Value.Item2,
				resultadoAmostra.Value.Item3,
				resultadoAmostra.Value.Item4);

			return pesos.Valores;
			//a função ProcessarAmostra calcula o resultado da amostra usando a classe Amostra, verifica se houve erro na previsão e, em caso afirmativo,
			//atualiza os pesos usando a classe Peso. Ela retorna os pesos atualizados como uma lista de valores.
		}
	}

	public class Entrada
	{
		public List<double> Valores { get; set; } = new();
		public int ValorDesejadoDeSaida { get; set; }
	}




	public class Peso
	{
		public List<double> Valores { get; set; } = new();

		//Isso declara um método estático público chamado AtualizarPesos. Este método recebe
		//quatro parâmetros: uma lista de pesosAntigos,, uma lista de valoresDaEntradaQueDeuErro,
		//um resultadoDaAmostraQueDeuErro do tipo double e um valorDesejadoDaSaidaQueDeuErro do tipo int.
		public static List<double> AtualizarPesos(List<double> pesosAntigos,
			List<double> valoresDaEntradaQueDeuErro,
			double resultadoDaAmostraQueDeuErro,
			int valorDesejadoDaSaidaQueDeuErro)
		{//será usada para armazenar os novos valores dos pesos.
			List<double> novosPesos = new();
			//que será usada para armazenar o resultado da multiplicação dos valores de entrada pelo resultado calculado.
			List<double> resultadoMultiplicaEntrada = new();

			double resultado = 0.05 * (valorDesejadoDaSaidaQueDeuErro - resultadoDaAmostraQueDeuErro);

			//foreach (var valor in valoresDaEntradaQueDeuErro): Aqui, estamos usando um
			//loop foreach para percorrer cada valor na lista valoresDaEntradaQueDeuErro.
			//Para cada valor na lista, ele é temporariamente armazenado na variável valor.
			foreach (var valor in valoresDaEntradaQueDeuErro)
			{
				//: Dentro do loop, estamos multiplicando o valor de entrada atual (valor) pelo
				//resultado do cálculo de erro (resultado) que foi calculado anteriormente. O resultado
				//dessa multiplicação é adicionado à lista resultadoMultiplicaEntrada. Isso está criando
				//uma lista de valores resultantes da multiplicação dos valores de entrada com o erro
				//calculado.
				resultadoMultiplicaEntrada.Add(valor * resultado);
			}
			//for tradicional para percorrer cada elemento na lista resultadoMultiplicaEntrada. A variável
			//i é usada como um índice para acessar cada elemento da lista.

			for (int i = 0; i < resultadoMultiplicaEntrada.Count; i++)
			{
				//Dentro do loop for, estamos calculando os novos pesos somando o valor correspondente
				//na lista resultadoMultiplicaEntrada (que foi multiplicado pelo erro) com o valor
				//correspondente nos pesosAntigos. O resultado dessa soma é adicionado à lista novosPesos
				//. Isso está atualizando os pesos com base nos erros calculados.
				novosPesos.Add(resultadoMultiplicaEntrada[i] + pesosAntigos[i]);
			}
			//novosPesos que contém os pesos atualizados com base nos erros. Essa lista representa os pesos ajustados
			//após a correção de uma amostra que deu erro.
			return novosPesos;
		}
	}

	public class PrintConsole
	{
		public static void PrintPesosIniciais(Peso pesos)
		{
			Console.WriteLine("Os valores dos pesos iniciais são:");
			foreach (var peso in pesos.Valores)
			{
				Console.WriteLine(peso);
			}
			Console.WriteLine();
		}

		public static void PrintPesosAtuais(Peso pesos, int iteracao)
		{
			Console.WriteLine($"Pesos atuais, iteracao numero: {iteracao}");
			foreach (var peso in pesos.Valores)
			{
				Console.WriteLine(peso);
			}
			Console.WriteLine();
		}

		public static void PrintPesosFinais(Peso pesos)
		{
			Console.WriteLine("Os valores dos pesos finais são:");
			foreach (var peso in pesos.Valores)
			{
				Console.WriteLine(peso);
			}
		}

		public static void PrintQuantidadeDeIteracoes(int quantidade)
		{
			Console.WriteLine();
			Console.WriteLine($"A quantidade de iteracoes foi: {quantidade}");
		}
	}

	public class Amostra
	{
		// Isso declara um método estático público chamado CalcularAmostra.
		// O método recebe três parâmetros: uma lista de valoresPeso, uma lista de valoresEntrada
		// e um valorSaidaDesejado

		//O método retorna uma tupla contendo uma lista de double para os valores de peso, uma lista de double para os valores de entrada,
		//um double para o resultado da amostra e um int para o valor de saída desejado. A tupla é nullable (?) para permitir que retorne null
		//em caso de resultado desejado.
		public static (List<double>, List<double>, double, int)? CalcularAmostra(List<double> valoresPeso,
			List<double> valoresEntrada, int valorSaidaDesejado)
		{// Inicializa a variável resultadoAmostra com o valor 0. Esta variável será usada para calcular o resultado da amostra multiplicando
		 // os valores de entrada pelos pesos correspondentes
			double resultadoAmostra = 0;

			for (int i = 0; i < valoresPeso.Count; i++)
			{
				//resultadoAmostra += (valoresEntrada[i] * valoresPeso[i]);: Dentro do loop,
				//estamos multiplicando o valor de entrada atual (valoresEntrada[i]) pelo peso
				//correspondente (valoresPeso[i]). O resultado dessa multiplicação é somado ao
				//resultadoAmostra. Isso está calculando a combinação linear dos valores de entrada
				//ponderados pelos pesos
				resultadoAmostra += (valoresEntrada[i] * valoresPeso[i]);
			}
			//bool resultadoDesejado = VerificarResultadoAmostra(resultadoAmostra, valorSaidaDesejado);: Aqui, estamos chamando a função
			//VerificarResultadoAmostra para verificar se o resultado da amostra atende ao valor de saída desejado. A função retorna true
			//se o resultado for positivo e false caso contrário.
			bool resultadoDesejado = VerificarResultadoAmostra(resultadoAmostra, valorSaidaDesejado);
			//if (!resultadoDesejado): Verifica se o resultado da amostra não atende ao valor de saída desejado.
			if (!resultadoDesejado)
			{
				//(List<double>, List<double>, double, int)? valoresDaAmostraQueFalhou = (...): Se o resultado não for o desejado, criamos uma tupla (List<double>, List<double>, double, int) que contém os valores de peso, valores de entrada,
				//resultado da amostra e valor de saída desejado que levaram a um erro na previsão.
				(List<double>, List<double>, double, int)? valoresDaAmostraQueFalhou =
					(valoresPeso, valoresEntrada, resultadoAmostra, valorSaidaDesejado);
				//return valoresDaAmostraQueFalhou;: Retornamos a tupla que contém as informações relevantes da amostra que falhou na previsão.
				return valoresDaAmostraQueFalhou;
			}
			//e o resultado da amostra atender ao valor de saída desejado, retornamos null para indicar que não houve erro na previsão.
			return null;
		}

		// O método recebe dois parâmetros: um double chamado resultado, que é o resultado da
		// combinação linear dos valores de entrada com os pesos, e um int chamado valorDesejado,
		// que é o valor de saída desejado (1 ou -1).
		private static bool VerificarResultadoAmostra(double resultado, int valorDesejado)
		{
			if (valorDesejado == 1)
			{
				return resultado > 0;
			}

			return resultado < 0;
		}
	}
}
