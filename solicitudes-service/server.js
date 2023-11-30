const bodyParser = require('body-parser')
const cors = require('cors')
const express = require('express')
const app = express()
const { MongoClient } = require('mongodb');
const { v4: uuidv4 } = require('uuid');

class Solicitud {
    id = '-'
    estado = 'Por evaluar'

    constructor(nombres, apellidoPaterno, apellidoMaterno, telefono, rut, especialidad, nombreDeLaUnidad, detalle, peticion) {
        this.Nombres = nombres
        this.ApellidoPaterno = apellidoPaterno
        this.ApellidoMaterno = apellidoMaterno
        this.Rut = rut
        this.Especialidad = especialidad
        this.NombreUnidad = nombreDeLaUnidad
        this.Telefono = telefono
        this.Detalle = detalle
        this.Id = uuidv4()
    }
}

const PORT = 40401

//For cross origin
app.use(cors())

// For parsing application/json
app.use(bodyParser.json())

app.get('/', (req, res) => {
    res.send('Hello World')
})

console.log('Connecting to MongoDB');

const url = 'mongodb://root:password@solicitudes-mongo:27017';

async function seedDB() {
    const client = new MongoClient(url);
    try {
        await client.connect();
        console.log('Connected to MongoDB');

        const db = client.db(dbName);
        const collection = db.collection('solicitudes');

        const solicitud1 = new Solicitud(
            'Juan Edmundo',
            'Perez',
            'Gonzalez',
            '17283652-3',
            'Asistencia Social',
            'Administrativo',
            '12345678',
            'Detalle de la solicitud'
        )
        solicitud1.id = '705d8b57-f80b-4eba-9c99-d63a5925ceb4'

        const solicitud2 = new Solicitud(
            'Pedro Pablo',
            'Perez',
            'Mancilla',
            '20734817-k',
            'Cardiologia',
            'Enfermeria',
            '12345678',
            'Detalle de la solicitud'
        )
        solicitud2.id = '8475ffdd-b5ac-4fc1-848d-887040517b41'

        const seedData = [
            solicitud1,
            solicitud2
        ];

        await collection.insertMany(seedData);
        console.log('Database seeded!');
    } finally {
        await client.close();
    }
}

seedDB().catch(console.error);

const dbName = 'myDatabase';

app.post('/solicitudes', async (req, res) => {
    const client = new MongoClient(url);
    try {
        await client.connect();
        console.log('Connected successfully to MongoDB');

        const db = client.db(dbName);
        const collection = db.collection('solicitudes');

        req.body.id = uuidv4();
        req.body.estado = 'Por evaluar';

        console.log(req.body);

        const result = await collection.insertOne(req.body);
        res.status(201).send(result);
    } catch (error) {
        res.status(500).send(error.toString());
    } finally {
        // Ensures that the client will close when you finish/error
        await client.close();
    }
});

app.get('/solicitudes', async (req, res) => {
    const client = new MongoClient(url);
    try {
        await client.connect();
        console.log('Connected successfully to MongoDB');
        
        const db = client.db(dbName);
        const collection = db.collection('solicitudes');
        
        const docs = await collection.find({}).toArray();
        res.status(200).send(docs);
    } catch (error) {
        res.status(500).send(error.toString());
    } finally {
        await client.close();
    }
});

app.put('/solicitudes/:id', async (req, res) => {
    const client = new MongoClient(url);
    try {
        await client.connect();
        console.log('Connected successfully to MongoDB');

        const db = client.db(dbName);
        const collection = db.collection('solicitudes');

        const params_id = req.params.id;
        const result = await collection.updateOne(
            { id: params_id },
            { $set: req.body }
        );
        res.status(200).send(result);
    } catch (error) {
        res.status(500).send(error.toString());
    } finally {
        await client.close();
    }
});

app.get('/solicitudes/:id', async (req, res) => {
    const client = new MongoClient(url);
    try {
        await client.connect();
        console.log('Connected successfully to MongoDB');

        const db = client.db(dbName);
        const collection = db.collection('solicitudes');

        const params_id = req.params.id;
        const document = await collection.findOne({ id: params_id });
        if (document) {
            res.status(200).send(document);
        } else {
            res.status(404).send('Document not found');
        }
    } catch (error) {
        res.status(500).send(error.toString());
    }
});



app.listen(PORT, () => {
    console.log(`Server running on http://0.0.0.0:${PORT}`);
});






// app.post('/solicitudes', (req, res) => {
//     console.log(req.body)
//     const solicitud = new Solicitud(
//         req.body.nombres,
//         req.body.apellidoPaterno,
//         req.body.apellidoMaterno,
//         req.body.telefono,
//         req.body.rut,
//         req.body.especialidad,
//         req.body.nombreDeLaUnidad,
//         req.body.detalle,
//         req.body.peticion
//     )
//     console.log(solicitud)
//     res.status(201).json({
//         message: 'Solicitud creada exitosamente',
//         solicitud: solicitud
//     })
// })