package com.example.parkamigo.ui

import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.compose.ui.text.font.FontWeight

@Composable
fun AdminMainMenu(onSeleccion: (String) -> Unit) {
    Column(
        modifier = Modifier
            .fillMaxWidth()
            .padding(16.dp),
        verticalArrangement = Arrangement.spacedBy(24.dp, Alignment.CenterVertically),
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Text("Menú Administrador", fontSize = 20.sp, fontWeight = FontWeight.Bold)

        MenuCard("Usuarios") { onSeleccion("usuarios") }
        MenuCard("Reservas") { onSeleccion("reservas") }
        MenuCard("Estacionamiento") { onSeleccion("estacionamiento") }
        MenuCard("Tarjetas") { onSeleccion("tarjetas") }
    }
}

@Composable
fun MenuCard(texto: String, onClick: () -> Unit) {
    Card(
        onClick = onClick,
        modifier = Modifier
            .fillMaxWidth(0.8f)
            .height(80.dp),
        elevation = CardDefaults.cardElevation(8.dp)
    ) {
        Box(contentAlignment = Alignment.Center, modifier = Modifier.fillMaxSize()) {
            Text(texto, fontSize = 18.sp, fontWeight = FontWeight.Medium)
        }
    }
}
