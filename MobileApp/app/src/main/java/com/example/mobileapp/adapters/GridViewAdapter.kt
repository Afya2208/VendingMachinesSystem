package com.example.mobileapp.adapters

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.TextView
import com.example.mobileapp.R
import com.example.mobileapp.databinding.TaskItemBinding
import com.example.mobileapp.models.Task

class GridViewAdapter(var cont: Context, var list: ArrayList<Task>) : ArrayAdapter<Task>(cont, 0, list) {
    override fun getView(position: Int, convertView: View?, parent: ViewGroup): View {

        var view = convertView
        var binding = TaskItemBinding.inflate(LayoutInflater.from(cont), parent, false);
        if (view == null) {
            view = binding.root
        }
        with(binding) {

        }

        var item = list[position]
        var titleView = view.findViewById<TextView>(R.id.taskTitleView)
        var textView = view.findViewById<TextView>(R.id.taskTextView)

        return view
    }
}