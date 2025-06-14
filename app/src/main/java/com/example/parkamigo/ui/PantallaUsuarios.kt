package com.example.parkamigo.ui

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.text.input.VisualTransformation
import androidx.compose.ui.unit.dp
import com.google.firebase.firestore.FirebaseFirestore
import kotlinx.coroutines.tasks.await
import com.example.parkamigo.model.Usuario

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun PantallaUsuarios(onBack: () -> Unit) {
    val db = FirebaseFirestore.getInstance()

    var listaUsuarios by remember { mutableStateOf(listOf<Usuario>()) }
    var buscando by remember { mutableStateOf("") }
    var usuarioSeleccionado by remember { mutableStateOf<Usuario?>(null) }
    var mostrarCrearUsuario by remember { mutableStateOf(false) }
    var cargando by remember { mutableStateOf(false) }
    var errorMsg by remember { mutableStateOf<String?>(null) }

    // Cargar lista de usuarios desde Firestore
    LaunchedEffect(Unit) {
        cargando = true
        try {
            val snapshot = db.collection("Registro-Usuario").get().await()
            listaUsuarios = snapshot.documents.mapNotNull { doc ->
                doc.toObject(Usuario::class.java)?.copy(id = doc.id)
            }
        } catch (e: Exception) {
            errorMsg = "Error al cargar usuarios: ${e.message}"
        }
        cargando = false
    }

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Usuarios") },
                navigationIcon = {
                    IconButton(onClick = onBack) {
                        Icon(Icons.Filled.ArrowBack, contentDescription = "Atrás")
                    }
                }
            )
        }
    ) { padding ->
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(padding)
                .padding(16.dp)
        ) {
            errorMsg?.let {
                Text(it, color = MaterialTheme.colorScheme.error)
            }

            OutlinedTextField(
                value = buscando,
                onValueChange = { buscando = it },
                label = { Text("Buscar por nombre, apellido o usuario") },
                modifier = Modifier.fillMaxWidth()
            )

            Spacer(modifier = Modifier.height(8.dp))

            Button(onClick = {
                mostrarCrearUsuario = true
                usuarioSeleccionado = null
            }) {
                Text("Crear Usuario")
            }

            Spacer(modifier = Modifier.height(8.dp))

            if (cargando) {
                CircularProgressIndicator()
            } else if (mostrarCrearUsuario) {
                CrearUsuarioForm(
                    onCancelar = { mostrarCrearUsuario = false },
                    onUsuarioCreado = { nuevoUsuario ->
                        listaUsuarios = listaUsuarios + nuevoUsuario
                        mostrarCrearUsuario = false
                    },
                    db = db
                )
            } else {
                val listaFiltrada = listaUsuarios.filter {
                    it.Nombre.contains(buscando, true) ||
                            it.Apellido.contains(buscando, true) ||
                            it.Nombre_de_usuario.contains(buscando, true)
                }

                if (listaFiltrada.isEmpty()) {
                    Text("No hay usuarios que coincidan.")
                } else {
                    Column(
                        modifier = Modifier
                            .fillMaxWidth()
                            .weight(1f)
                    ) {
                        listaFiltrada.forEach { usuario ->
                            Text(
                                text = "${usuario.Nombre} ${usuario.Apellido} (${usuario.Nombre_de_usuario})",
                                modifier = Modifier
                                    .fillMaxWidth()
                                    .clickable { usuarioSeleccionado = usuario }
                                    .padding(8.dp)
                            )
                            Divider()
                        }
                    }
                }

                Spacer(modifier = Modifier.height(8.dp))

                usuarioSeleccionado?.let { usuario ->
                    EditarUsuarioForm(
                        usuario = usuario,
                        onGuardar = { actualizado ->
                            cargando = true
                            errorMsg = null
                            if (actualizado.id.isNotBlank()) {
                                db.collection("Registro-Usuario")
                                    .document(actualizado.id)
                                    .set(actualizado)
                                    .addOnSuccessListener {
                                        listaUsuarios = listaUsuarios.map {
                                            if (it.id == actualizado.id) actualizado else it
                                        }
                                        usuarioSeleccionado = actualizado
                                        cargando = false
                                    }
                                    .addOnFailureListener {
                                        errorMsg = "Error al guardar: ${it.message}"
                                        cargando = false
                                    }
                            } else {
                                errorMsg = "El ID del usuario está vacío."
                                cargando = false
                            }
                        }
                    )
                }
            }
        }
    }
}

@Composable
fun EditarUsuarioForm(usuario: Usuario, onGuardar: (Usuario) -> Unit) {
    var nombre by remember { mutableStateOf(usuario.Nombre) }
    var apellido by remember { mutableStateOf(usuario.Apellido) }
    var usuarioNombre by remember { mutableStateOf(usuario.Nombre_de_usuario) }
    var celular by remember { mutableStateOf(usuario.Numero_de_celular) }
    var contrasena by remember { mutableStateOf(usuario.Contraseña) }
    var mostrarContrasena by remember { mutableStateOf(false) }
    var editado by remember { mutableStateOf(false) }

    Column(Modifier.fillMaxWidth().padding(8.dp)) {
        Text("Editar usuario", style = MaterialTheme.typography.titleMedium)

        OutlinedTextField(
            value = nombre,
            onValueChange = { nombre = it; editado = true },
            label = { Text("Nombre") },
            modifier = Modifier.fillMaxWidth()
        )
        OutlinedTextField(
            value = apellido,
            onValueChange = { apellido = it; editado = true },
            label = { Text("Apellido") },
            modifier = Modifier.fillMaxWidth()
        )
        OutlinedTextField(
            value = usuarioNombre,
            onValueChange = { usuarioNombre = it; editado = true },
            label = { Text("Nombre de usuario") },
            modifier = Modifier.fillMaxWidth()
        )
        OutlinedTextField(
            value = celular,
            onValueChange = { celular = it; editado = true },
            label = { Text("Celular") },
            keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Phone),
            modifier = Modifier.fillMaxWidth()
        )
        OutlinedTextField(
            value = contrasena,
            onValueChange = { contrasena = it; editado = true },
            label = { Text("Contraseña") },
            visualTransformation = if (mostrarContrasena) VisualTransformation.None else PasswordVisualTransformation(),
            trailingIcon = {
                val icon = if (mostrarContrasena) Icons.Filled.Visibility else Icons.Filled.VisibilityOff
                IconButton(onClick = { mostrarContrasena = !mostrarContrasena }) {
                    Icon(icon, contentDescription = null)
                }
            },
            modifier = Modifier.fillMaxWidth()
        )

        Spacer(modifier = Modifier.height(8.dp))

        Button(
            onClick = {
                if (nombre.isNotBlank() && apellido.isNotBlank() && usuarioNombre.isNotBlank()) {
                    onGuardar(
                        usuario.copy(
                            Nombre = nombre,
                            Apellido = apellido,
                            Nombre_de_usuario = usuarioNombre,
                            Numero_de_celular = celular,
                            Contraseña = contrasena
                        )
                    )
                    editado = false
                }
            },
            enabled = editado,
            modifier = Modifier.fillMaxWidth()
        ) {
            Text("Guardar cambios")
        }
    }
}

@Composable
fun CrearUsuarioForm(
    onCancelar: () -> Unit,
    onUsuarioCreado: (Usuario) -> Unit,
    db: FirebaseFirestore
) {
    var nombre by remember { mutableStateOf("") }
    var apellido by remember { mutableStateOf("") }
    var usuarioNombre by remember { mutableStateOf("") }
    var celular by remember { mutableStateOf("") }
    var contrasena by remember { mutableStateOf("") }
    var mostrarContrasena by remember { mutableStateOf(false) }
    var cargando by remember { mutableStateOf(false) }
    var errorMsg by remember { mutableStateOf<String?>(null) }

    Column(Modifier.fillMaxWidth().padding(8.dp)) {
        Text("Crear nuevo usuario", style = MaterialTheme.typography.titleMedium)

        errorMsg?.let {
            Text(it, color = MaterialTheme.colorScheme.error)
        }

        OutlinedTextField(value = nombre, onValueChange = { nombre = it }, label = { Text("Nombre") }, modifier = Modifier.fillMaxWidth())
        OutlinedTextField(value = apellido, onValueChange = { apellido = it }, label = { Text("Apellido") }, modifier = Modifier.fillMaxWidth())
        OutlinedTextField(value = usuarioNombre, onValueChange = { usuarioNombre = it }, label = { Text("Nombre de usuario") }, modifier = Modifier.fillMaxWidth())
        OutlinedTextField(value = celular, onValueChange = { celular = it }, label = { Text("Celular") }, keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Phone), modifier = Modifier.fillMaxWidth())
        OutlinedTextField(
            value = contrasena,
            onValueChange = { contrasena = it },
            label = { Text("Contraseña") },
            visualTransformation = if (mostrarContrasena) VisualTransformation.None else PasswordVisualTransformation(),
            trailingIcon = {
                val icon = if (mostrarContrasena) Icons.Filled.Visibility else Icons.Filled.VisibilityOff
                IconButton(onClick = { mostrarContrasena = !mostrarContrasena }) {
                    Icon(icon, contentDescription = null)
                }
            },
            modifier = Modifier.fillMaxWidth()
        )

        Spacer(modifier = Modifier.height(8.dp))

        Row {
            Button(
                onClick = {
                    if (nombre.isBlank() || apellido.isBlank() || usuarioNombre.isBlank()) {
                        errorMsg = "Por favor llena los campos obligatorios"
                        return@Button
                    }
                    cargando = true
                    errorMsg = null
                    val nuevoUsuario = Usuario(
                        Nombre = nombre,
                        Apellido = apellido,
                        Nombre_de_usuario = usuarioNombre,
                        Numero_de_celular = celular,
                        Contraseña = contrasena
                    )
                    db.collection("Registro-Usuario")
                        .add(nuevoUsuario)
                        .addOnSuccessListener {
                            onUsuarioCreado(nuevoUsuario)
                            cargando = false
                        }
                        .addOnFailureListener {
                            errorMsg = "Error al crear usuario: ${it.message}"
                            cargando = false
                        }
                },
                enabled = !cargando,
                modifier = Modifier.weight(1f)
            ) {
                Text("Crear")
            }

            Spacer(modifier = Modifier.width(8.dp))

            Button(
                onClick = onCancelar,
                enabled = !cargando,
                modifier = Modifier.weight(1f)
            ) {
                Text("Cancelar")
            }
        }

        if (cargando) {
            Spacer(modifier = Modifier.height(8.dp))
            CircularProgressIndicator()
        }
    }
}


