import express from "express";
import cors from "cors";
import bodyParser from "body-parser";
import dotenv from "dotenv";
import { GoogleGenerativeAI, SchemaType } from "@google/generative-ai";

dotenv.config();

const app = express();
app.use(cors());
app.use(bodyParser.json());

const genAI = new GoogleGenerativeAI(process.env.GEMINI_API_KEY);

// Função para validar quais modelos estão disponíveis para sua chave
async function listModels() {
    const models = await genAI.getGenerativeModel({ model: "models/gemini-1.5-flash" }); 
    // Se o de cima falhar, você verá no log.
}

// Configuração do NPC de Ansiedade
const promptSistema = `
Você é um NPC que sofre de ansiedade social severa.
Sinta medo de julgamento e interprete frases neutras como ameaças.
Reaja à empatia melhorando seu nível de abertura.
Reaja à agressividade se fechando totalmente.
`;

const model = genAI.getGenerativeModel({
  // Use exatamente este nome. Se der erro, tente "gemini-1.5-flash-latest"
  model: "gemini-3.1-flash-lite", 
  systemInstruction: promptSistema,
  generationConfig: {
    responseMimeType: "application/json",
    responseSchema: {
      type: SchemaType.OBJECT,
      properties: {
        dialogo: { type: SchemaType.STRING },
        nivel_abertura: { type: SchemaType.NUMBER },
        emocao_detectada: { type: SchemaType.STRING },
      },
      required: ["dialogo", "nivel_abertura", "emocao_detectada"],
    },
  },
});

app.post("/chat", async (req, res) => {
  try {
    const { mensagem } = req.body;
    console.log("Jogador disse:", mensagem);

    const result = await model.generateContent(mensagem);
    const response = await result.response;
    const text = response.text();

    console.log("IA respondeu:", text);
    res.send(text);
  } catch (erro) {
    // Aqui capturamos o erro do Google e mostramos no console do Node
    console.error("ERRO NA API DO GOOGLE:", erro.message);
    
    // Enviamos um JSON de erro para a Unity não dar NullReference
    res.status(500).json({
        dialogo: "Eu... eu não consigo falar agora...",
        nivel_abertura: 0.1,
        emocao_detectada: "ansioso"
    });
  }
});

app.listen(3000, () => {
  console.log("Servidor rodando em http://localhost:3000");
});