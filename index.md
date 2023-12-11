---
title: Azure OpenAI Exercises
permalink: index.html
layout: home
---

# Azure OpenAI Exercises

The following exercises are designed to support the modules on [Microsoft Learn](https://learn.microsoft.com/training/browse/?terms=OpenAI).


{% assign labs = site.pages | where_exp:"page", "page.url contains '/Instructions/Exercises'" %}
{% for activity in labs  %}
- [{{ activity.lab.title }}]({{ site.github.url }}{{ activity.url }})
{% endfor %}
