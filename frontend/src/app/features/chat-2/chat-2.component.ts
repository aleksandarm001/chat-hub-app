import { Component, OnInit } from '@angular/core';
import { ChatService } from 'src/app/services/chat.service';
import { Message } from './message';
import { User } from '../models/user';
import { UsersService } from 'src/app/services/users.service';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat-2.component.html',
  styleUrls: ['./chat-2.component.css'],
})
export class Chat2Component implements OnInit {
  messages: Message[] = [];
  users: User[] = [];
  activeUserId: number | null = null;
  currentUserId: number = 0;
  newMessage = '';

  constructor(private chatService: ChatService,
    private userService: UsersService,
    private authService: AuthService) {}

  async ngOnInit() {
    this.authService.user$.subscribe({
      next: (user) => {
        this.currentUserId = user.id;
        console.log("current user id: ", this.currentUserId);

        this.userService.getUsers().subscribe({
          next: (users) => {
            this.users = users.filter(user => user.id !== this.currentUserId);
          },
          error: (err) => {
            console.error('Error fetching users:', err);
          },
        });
      },
      error: (err) => {
        console.error('Error fetching user data:', err);
      }
    });

    await this.chatService.connect();
    await this.chatService.registerUser(this.currentUserId);

    this.chatService.addMessageListener((senderId, messageContent) => {
      const receivedMessage: Message = {
        senderId,
        receiverId: this.currentUserId,
        content: messageContent,
        timestamp: new Date(),
      };
      this.messages.push(receivedMessage);
      console.log(receivedMessage);
    });

    this.chatService.addUserNotAvailableListener((receiverId) => {
      //alert(`User with ID ${receiverId} is not available.`);
    });

    this.chatService.addChatStartedListener((userId, targetUserId) => {
      if (userId === this.currentUserId) {
        console.log(`Chat started with ${targetUserId}`);
      }
    });
  }

  onUserClick(targetUserId: number): void {
    this.activeUserId = targetUserId;
        console.log(this.activeUserId, " ", this.currentUserId);
    this.chatService.getMessages(this.currentUserId, targetUserId).subscribe({
      next: (messages) => {
        this.messages = messages; 
        this.chatService.startChat(this.currentUserId, targetUserId);
        console.log(this.messages);
      },
      error: (err) => {
        console.error('Error fetching messages:', err);
      },
    });
  }

  setActiveUser(userId: number) {
    this.activeUserId = userId;
  }

  fetchMessages(currentUserId: number, targetUserId: number) {
    this.chatService.getMessages(currentUserId, targetUserId).subscribe({
      next: (messages) => {
        this.messages = [...this.messages, ...messages]; 
      },
      error: (err) => {
        console.error('Error fetching messages:', err);
      },
    });
  }


  async sendMessage() {
    if (this.activeUserId && this.newMessage.trim()) {
      const message: Message = {
        senderId: this.currentUserId,
        receiverId: this.activeUserId,
        content: this.newMessage,
        timestamp: new Date(),
      };

      await this.chatService.sendMessage(this.activeUserId, this.currentUserId, this.newMessage);

      this.messages.push(message);
      this.newMessage = '';
    }
  }
}