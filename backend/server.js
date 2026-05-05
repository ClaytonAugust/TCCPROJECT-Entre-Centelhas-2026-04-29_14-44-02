import express from "express";
import cors from "cors";
import bodyParser from "body-parser";
import dotenv from "dotenv";
import OpenAI from "openai";

dotenv.config();

const app = express();
app.use(cors());
app.use(bodyParser.json());

const client = new OpenAI({
  apiKey: process.env.OPENAI_API_KEY,
});

// ROTA PRINCIPAL
app.post("/chat", async (req, res) => {
  try {
    const { mensagem } = req.body;

    const resposta = await client.chat.completions.create({
      model: "gpt-4o-mini",
      messages: [
        {
          role: "system",
          content: `
Você é um NPC com ansiedade social.

Responda SEMPRE em JSON no formato:
{
  "dialogo": "...",
  "nivel_abertura": 0.0 a 1.0,
  "emocao_detectada": "ansioso | defensivo | reflexivo | calmo"
}

Regras:
- Jogador agressivo → você se fecha (nivel baixo)
- Jogador empático → você melhora (nivel alto)
          `,
        },
        {
          role: "user",
          content: mensagem,
        },
      ],
    });

    const texto = resposta.choices[0].message.content;

    res.send(texto);
  } catch (erro) {
    console.error(erro);
    res.status(500).send("Erro no servidor");
  }
});

app.listen(3000, () => {
  console.log("Servidor rodando em http://localhost:3000");
});