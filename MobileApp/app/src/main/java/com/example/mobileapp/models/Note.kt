package com.example.mobileapp.models

import android.net.Uri
import java.time.LocalDateTime

class Note {
    var title:String = ""
    var text:String = ""
    var materialUri:Uri = Uri.EMPTY
    var dateTime:LocalDateTime = LocalDateTime.now()
}