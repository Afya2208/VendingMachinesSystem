package com.example.mobileapp.adapters

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import androidx.recyclerview.widget.RecyclerView
import com.example.mobileapp.R
import com.example.mobileapp.databinding.TaskItemBinding
import com.example.mobileapp.models.Task

class TaskAdapter(var cont: Context, var list: ArrayList<Task>) : ArrayAdapter<Task>(cont, R.layout.task_item, list) {

    override fun getView(position: Int, convertView: View?, parent: ViewGroup): View {
        var binding: TaskItemBinding = TaskItemBinding.inflate(LayoutInflater.from(cont), parent, false)
        var view: View? = convertView
        if (view == null) {
            view = LayoutInflater.from(cont).inflate(R.layout.task_item, parent, false)
        }
        return view
    }
}