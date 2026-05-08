import express from "express";
import cors from "cors";
import bodyParser from "body-parser";
import dotenv from "dotenv";
import { GoogleGenerativeAI, SchemaType } from "@google/generative-ai";

dotenv.config();

const app = express();
app.use(cors());
app.use(bodyParser.json());

// Inicializa o SDK usando a chave do seu .env
const genAI = new GoogleGenerativeAI(process.env.GEMINI_API_KEY);

// Seu prompt estruturado
const promptSistema = `
Você representa ansiedade social.

Você sente medo de julgamento.
Você evita contato social.
Você interpreta situações neutras como ameaças.

Se o jogador for empático:
- você melhora lentamente
- fica mais confortável
- ganha confiança

Se o jogador for agressivo:
- você se fecha
- evita conversar
- aumenta ansiedade
`;

// Configuração do modelo forçando a saída em JSON
const model = genAI.getGenerativeModel({
  model: "gemini-1.5-flash",
  systemInstruction: promptSistema,
  generationConfig: {
    responseMimeType: "application/json",
    responseSchema: {
      type: SchemaType.OBJECT,
      properties: {
        dialogo: {
          type: SchemaType.STRING,
          description: "A fala do NPC",
        },
        nivel_abertura: {
          type: SchemaType.NUMBER,
          description: "Float de 0.0 a 1.0 indicando conforto",
        },
        emocao_detectada: {
          type: SchemaType.STRING,
          description: "ansioso, defensivo, reflexivo ou calmo",
        },
      },
      required: ["dialogo", "nivel_abertura", "emocao_detectada"],
    },
  },
});

app.post("/chat", async (req, res) => {
  try {
    const { mensagem } = req.body;

    // Envia a mensagem do jogador para a IA
    const result = await model.generateContent(mensagem);
    
    // O texto já virá garantido no formato JSON estruturado
    const textoJson = result.response.text(); 

    res.send(textoJson);
  } catch (erro) {
    console.error(erro);
    res.status(500).send("Erro no servidor");
  }
});

app.listen(3000, () => {
  console.log("Servidor rodando em http://localhost:3000");
});